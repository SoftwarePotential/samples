/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */
using DemoApp.Common;
using Microsoft.Win32;
using Sp.Agent;
using Sp.Agent.Licensing;
using Sp.Agent.Storage;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace DemoApp.Activation
{
	public class OfflineActivationModel : ViewModelBase, IDataErrorInfo
	{
		public RelayCommand GenerateRequestCommand { get; set; }
		public RelayCommand CopyToClipboardCommand { get; set; }
		public RelayCommand BrowseAndInstallCommand { get; set; }

		string _activationKey;
		public string ActivationKey
		{
			get { return _activationKey; }
			set
			{
				_activationKey = value;
				GenerateRequestCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "ActivationKey" );
			}
		}

		string _activationRequest;
		public string ActivationRequest
		{
			get { return _activationRequest; }
			set
			{
				_activationRequest = value;
				CopyToClipboardCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "ActivationRequest" );
			}
		}


		string _licenseInstallResult;
		public string LicenseInstallResult
		{
			get { return _licenseInstallResult; }
			set
			{
				_licenseInstallResult = value;
				OnPropertyChanged( "LicenseInstallResult" );
			}
		}

		bool _lastInstallSucceedeed;
		public bool LastInstallSucceeded
		{
			get { return _lastInstallSucceedeed; }
			set
			{
				_lastInstallSucceedeed = value;
				OnPropertyChanged( "LastInstallSucceeded" );
			}
		}

		public OfflineActivationModel()
		{
			GenerateRequestCommand = new RelayCommand( GenerateRequest, CanGenerateRequest );
			CopyToClipboardCommand = new RelayCommand( CopyToClipboard, CanCopyToClipboard );
			BrowseAndInstallCommand = new RelayCommand( BrowseAndInstallLicense );
		}

		void GenerateRequest()
		{
			ActivationRequest = SpAgent.Product.Activation.Advanced().CreateManualActivationRequest( ActivationKey, null );
		}

		bool CanGenerateRequest()
		{
			return !string.IsNullOrEmpty( ActivationKey ) && ActivationKey.Length == ActivationKeyRequiredLength && IsActivationKeyWellFormed();
		}

		void CopyToClipboard()
		{
			Clipboard.SetData( DataFormats.Text, ActivationRequest );
		}

		bool CanCopyToClipboard()
		{
			return !string.IsNullOrEmpty( ActivationRequest );
		}

		void BrowseAndInstallLicense()
		{
			var fileDialog = new OpenFileDialog() { DefaultExt = ".bin", Filter = "License Files (*.bin)|*.bin", };

			bool? result = fileDialog.ShowDialog();

			if ( result != true)
				return;		

			var license = File.ReadAllBytes( fileDialog.FileName );
			InstallLicense( license );
		}

		void InstallLicense( byte[] license )
		{
			LastInstallSucceeded = false;
			try
			{
				SpAgent.Product.Stores.Install( license );
				LicenseInstallResult = "Success: The license has been successfully installed.";
				LastInstallSucceeded = true;
			}
			catch ( LicenseRevisionException )
			{
				LicenseInstallResult = "Error: There is a newer version of the license already installed.";
			}
			catch ( NonmatchingProductIdException )
			{
				LicenseInstallResult = string.Format( "Error: The given license is not for the product: {0} version: {1}.", SpAgent.Product.ProductName, SpAgent.Product.ProductVersion );
			}
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