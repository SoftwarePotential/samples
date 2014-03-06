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
using System;
using System.ComponentModel;
using System.Windows;

namespace DemoApp.Activation
{
	public class ActivationModel : ViewModelBase, IDataErrorInfo
	{
		public RelayCommand ActivationCommand { get; set; }
		string _activationKey;

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
			ActivationCommand = new RelayCommand( Activate, CanActivate );
		}

		void Activate()
		{
			try
			{
				ActivateOnline();
				MessageBox.Show( "Successfully activated license with activation key " + ActivationKey );
				//Close();
			}
			catch ( Exception exc )
			{
				MessageBox.Show( "Error: " + exc.Message );
			}
		}

		bool CanActivate()
		{
			return !string.IsNullOrEmpty( ActivationKey ) && ActivationKey.Length == ActivationKeyRequiredLength && IsActivationKeyWellFormed();
		}

		public void ActivateOnline()
		{
			SpAgent.Product.Activation.OnlineActivate( ActivationKey );
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
}