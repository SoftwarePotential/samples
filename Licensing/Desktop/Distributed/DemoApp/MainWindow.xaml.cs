/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using DemoApp.Common;
using DemoApp.Properties;
using DemoApp.Checkout;
using DemoApp.Configuration;

namespace DemoApp
{
	public partial class MainWindow : Window, IDisplayState
	{
		const string TitlePrefix = "Distributed Demo App";

		public MainWindow()
		{
			InitializeComponent();

			if ( !Settings.Default.FirstRunLicensingConfigurationFinished )
			{
				var dialog = new ConfigurationDialog();
				dialog.ShowDialog();
			}
		}

		void Configure_Click( object sender, RoutedEventArgs e )
		{
			var dialog = new ConfigurationDialog { Owner = this };
			dialog.ShowDialog();
		}

		void Checkout_Click( object sender, RoutedEventArgs e )
		{
			var dialog = new CheckoutDialog { Owner = this };
			dialog.ShowDialog();
		}

		public void Navigate( Page page )
		{
			((ViewModelBase)page.DataContext).DisplayState = this;
			MainFrame.Navigate( page );
		}

		public void NotifyUser( object message )
		{
			MessageBox.Show( message.ToString() );
		}

		public bool Warn( object message )
		{
			return MessageBox.Show( message.ToString(), "Please confirm", MessageBoxButton.YesNo ) == MessageBoxResult.Yes;
		}

		public void Exit()
		{
			Close();
		}

		void MainFrame_Navigated( object sender, NavigationEventArgs e )
		{
			var targetPage = (Page)MainFrame.Content;
			Title = TitlePrefix + " - " + targetPage.Title;
			((ViewModelBase)targetPage.DataContext).DisplayState = this;
		}

		void CommandBinding_OnExecuted( object sender, ExecutedRoutedEventArgs e )
		{
			var uri = new Uri( (string)e.Parameter, UriKind.Relative );
			MainFrame.Navigate( uri );
		}

		void CommandBinding_OnCanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
	}
}
