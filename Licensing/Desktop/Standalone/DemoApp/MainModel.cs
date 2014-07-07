using DemoApp.Common;
using Sp.Agent;
using System;
using System.Windows;
using System.Windows.Threading;

namespace DemoApp
{
	public class MainModel : ViewModelBase
	{
		public RelayCommand Feature1Command { get; set; }
		public RelayCommand Feature2Command { get; set; }

		public MainModel()
		{
			Feature1Command = new RelayCommand( Feature1, CanExecuteFeature1 );
			Feature2Command = new RelayCommand( Feature2, CanExecuteFeature2 );
			SpAgent.Product.Stores.LicenseInstalled += OnLicenseInstalled;
		}

		//[Demo_10.Features.Feature1]
		[Aidansmed_1.Features.Crop]
		void Feature1()
		{
			DisplayState.NotifyUser( "Feature 1 accessed successfully" );
		}

		bool CanExecuteFeature1()
		{
		//	return SpAgent.Product.LocalFeatures.ValidContains( Demo_10.Features.Feature1.Name );
			return SpAgent.Product.LocalFeatures.ValidContains( Aidansmed_1.Features.Crop.Name );
		}

	//	[Demo_10.Features.Feature2]
		[Aidansmed_1.Features.Rotate]
		void Feature2()
		{
			DisplayState.NotifyUser( "Feature 2 accessed successfully" );
		}

		bool CanExecuteFeature2()
		{
			//return SpAgent.Product.LocalFeatures.ValidContains( Demo_10.Features.Feature2.Name );

			return SpAgent.Product.LocalFeatures.ValidContains( Aidansmed_1.Features.Rotate.Name );
		}

		void OnLicenseInstalled( object sender, EventArgs e )
		{
			((Window)DisplayState).Dispatcher.BeginInvoke( (Action)(() =>
			{
				Feature1Command.RaiseCanExecuteChanged();
				Feature2Command.RaiseCanExecuteChanged();
			}) );
		}
	}
}