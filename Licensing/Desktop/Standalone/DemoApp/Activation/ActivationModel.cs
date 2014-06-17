/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Common;
using Sp.Agent;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

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
		}

		bool CanActivate()
		{
			return !string.IsNullOrEmpty( ActivationKey ) && ActivationKey.Length == ActivationKeyRequiredLength && IsActivationKeyWellFormed() && !IsActivationInProgress;
		}

		void ActivateOnline()
		{
			SetActivationInProgress( true );
			LastActivationResultMessage = string.Empty;
			var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
			SpAgent.Product.Activation.OnlineActivateAsync( ActivationKey )
				.ContinueWith( task => OnActivationComplete( task, ActivationKey ), CancellationToken.None, TaskContinuationOptions.None, uiContext );
		}

		void OnActivationComplete( Task task, string activationKey )
		{
			SetActivationInProgress( false );
			if ( task.IsFaulted )
			{
				string errorMessage = task.Exception.Flatten().InnerException.Message;
				LastActivationResultMessage = "Error: " + errorMessage;
			}
			else
			{
				LastActivationResultMessage = "Successfully activated license with activation key " + activationKey;
				LastActivationSucceeded = true;
				ActivationKey = string.Empty;
			}
		}

		void SetActivationInProgress( bool isInProgress )
		{
			IsActivationInProgress = isInProgress;
		}

		bool IsActivationKeyWellFormed()
		{
			return SpAgent.Product.Activation.IsWellFormedKey( ActivationKey );
		}

		static int ActivationKeyRequiredLength
		{
			get { return 29; }
		}

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
	}
}