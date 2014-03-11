/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Common;
using DemoApp.Configuration;
using Sp.Agent;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DemoApp.Activation
{
	public class ActivationModel : ViewModelBase, IDataErrorInfo
	{
		public RelayCommand ActivationCommand { get; set; }
		public RelayCommand CancelCommand { get; set; }
		string _activationKey;

		bool _activationInProgress;
		public bool IsActivationInProgress
		{
			get { return _activationInProgress; }
			set
			{
				_activationInProgress = value;
				ActivationCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "IsActivationInProgress" );
			}
		}

		string _lastActivationResultMessage;
		public string LastActivationResultMessage
		{
			get { return _lastActivationResultMessage; }
			set
			{
				_lastActivationResultMessage = value;
				OnPropertyChanged( "LastActivationResultMessage" );
			}
		}

		bool _lastActivationSucceedeed;
		public bool LastActivationSucceeded
		{
			get { return _lastActivationSucceedeed; }
			set
			{
				_lastActivationSucceedeed = value;
				OnPropertyChanged( "LastActivationSucceeded" );
			}
		}
		public string ActivationKey
		{
			get { return _activationKey; }
			set
			{
				_activationKey = value;
				ActivationCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "ActivationKey" );
			}
		}

		public ActivationModel()
		{
			ActivationCommand = new RelayCommand( ActivateOnline, CanActivate );
			CancelCommand = new RelayCommand( () => DisplayState.Navigate( new MainPage() ) );
		}
			
		bool CanActivate()
		{
			return !string.IsNullOrEmpty( ActivationKey ) && ActivationKey.Length == ActivationKeyRequiredLength && IsActivationKeyWellFormed() && !IsActivationInProgress;
		}

		void ActivateOnline()
		{
			var activationModel = this;
			SetActivationInProgress( true );

			var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
			SpAgent.Product.Activation.OnlineActivateAsync( ActivationKey )
				.ContinueWith( task =>
				{
					SetActivationInProgress( false );

					if ( task.IsFaulted )
					{
						string errorMessage = task.Exception.Flatten().InnerException.Message;
						LastActivationResultMessage = "Error: " + errorMessage;
					}
					else
					{
						LastActivationResultMessage = "Successfully activated license with activation key " + activationModel.ActivationKey;
						LastActivationSucceeded = true;
					}

				}, CancellationToken.None, TaskContinuationOptions.None, uiContext );
		}

		void SetActivationInProgress( bool isInProgress )
		{
			IsActivationInProgress = isInProgress;
			// Notify the CommandManager that ActivationCommand CanExecute computed value could have changed (Activation button visibility depends on that)
			CommandManager.InvalidateRequerySuggested();
		}

		bool IsActivationKeyWellFormed()
		{
			return SpAgent.Product.Activation.IsWellFormedKey( ActivationKey );
		}

		public int ActivationKeyRequiredLength
		{
			get { return 29; }
		}

		#region IDataErrorInfo Members
		public string this[ string columnName ]
		{
			get
			{
				string result = null;
				if ( columnName == "ActivationKey" )
				{
					if ( string.IsNullOrEmpty( ActivationKey ) )
						result = "Please enter an activation key";
					else if ( ActivationKey.Length != ActivationKeyRequiredLength )
						result = string.Format( "Activation key should be exactly {0} characters long", ActivationKeyRequiredLength );
					else if ( !IsActivationKeyWellFormed() )
						result = "Activation key is not in the correct format";
				}
				return result;
			}
		}

		public string Error
		{
			get { return null; }
		}
		#endregion

	}

	#region Converters

	[ValueConversion( typeof( bool ), typeof( bool ) )]
	public class InverseBooleanConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			if ( targetType != typeof( bool ) )
				throw new InvalidOperationException( "The target must be a boolean" );

			return !(bool)value;
		}

		public object ConvertBack( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}

	[ValueConversion( typeof( bool ), typeof( bool ) )]
	public class BooleanErrorMessageColorConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			if ( value == null )
				return "Red";

			bool valueAsBool = (bool)value;
			return valueAsBool ? "Green" : "Red";
		}

		public object ConvertBack( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}

	[ValueConversion( typeof( bool ), typeof( bool ) )]
	public class InverseBooleanVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			var invertedBool = _inverseBooleanConverter.Convert( value, typeof( bool ), parameter, culture );
			return _booleanToVisibilityConverter.Convert( invertedBool, targetType, parameter, culture );
		}

		public object ConvertBack( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			throw new NotSupportedException();
		}

		readonly InverseBooleanConverter _inverseBooleanConverter = new InverseBooleanConverter();
		readonly BooleanToVisibilityConverter _booleanToVisibilityConverter = new BooleanToVisibilityConverter();
	}
	#endregion
}