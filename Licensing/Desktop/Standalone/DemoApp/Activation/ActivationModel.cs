/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System.Threading.Tasks;
using Sp.Agent;
using System;
using System.ComponentModel;

namespace DemoApp.Activation
{
	public class ActivationModel : IDataErrorInfo, INotifyPropertyChanged
	{
		string _activationKey;
		public string ActivationKey
		{
			get { return _activationKey; }
			set
			{
				_activationKey = value;
				OnPropertyChanged( "ActivationKey" );
			}
		}

		bool _activationInProgress;
		public bool IsActivationInProgress
		{
			get { return _activationInProgress; }
			set
			{
				_activationInProgress = value;
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

		public Task ActivateOnlineAsync()
		{
			return SpAgent.Product.Activation.OnlineActivateAsync( ActivationKey );
		}

		static bool IsActivationKeyWellFormed( string activationKey )
		{
			return SpAgent.Product.Activation.IsWellFormedKey( activationKey );
		}

		public static int ActivationKeyRequiredLength
		{
			get { return 29; }
		}

		static string ValidateActivationKey( string activationKey )
		{
			string result = null;
			if ( string.IsNullOrEmpty( activationKey ) )
				result = "Please enter an activation key";
			else if ( activationKey.Length != ActivationKeyRequiredLength )
				result = string.Format( "Activation key should be exactly {0} characters long", ActivationKeyRequiredLength );
			else if ( !IsActivationKeyWellFormed( activationKey ) )
				result = "Activation key is not in the correct format";
			return result;
		}

		#region IDataErrorInfo Members
		public string this[ string columnName ]
		{
			get
			{
				string result = null;
				if ( columnName == "ActivationKey" )
				{
					result = ValidateActivationKey( ActivationKey );
				}
				return result;
			}
		}

		public string Error
		{
			get { return null; }
		}
		#endregion

		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged( String propertyName )
		{
			if ( PropertyChanged != null )
			{
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
			}
		}
		#endregion
	}
}
