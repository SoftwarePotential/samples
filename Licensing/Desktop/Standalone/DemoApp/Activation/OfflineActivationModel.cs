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
		public RelayCommand BrowseFileSystemCommand { get; set; }
		public RelayCommand InstallLicenseFileCommand { get; set; }
		string _activationKey;

		public string OfflineActivationKey
		{
			get { return _activationKey; }
			set
			{
				_activationKey = value;
				GenerateRequestCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "OfflineActivationKey" );
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

		string _licenseFilePath;
		public string LicenseFilePath
		{
			get { return _licenseFilePath; }
			set
			{
				_licenseFilePath = value;
				InstallLicenseFileCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "LicenseFilePath" );
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
			BrowseFileSystemCommand = new RelayCommand( BrowseFileSystem );
			InstallLicenseFileCommand = new RelayCommand( InstallLicenseFile, CanInstallLicenseFile );
		}

		void GenerateRequest()
		{
			ActivationRequest = SpAgent.Product.Activation.Advanced().CreateManualActivationRequest( OfflineActivationKey, null );
		}

		bool CanGenerateRequest()
		{
			return !string.IsNullOrEmpty( OfflineActivationKey ) && OfflineActivationKey.Length == ActivationKeyRequiredLength && IsActivationKeyWellFormed();
		}

		void CopyToClipboard()
		{
			Clipboard.SetText( ActivationRequest );
		}

		bool CanCopyToClipboard()
		{
			return !string.IsNullOrEmpty( ActivationRequest );
		}

		void BrowseFileSystem()
		{
			var fileDialog = new OpenFileDialog() { DefaultExt = ".bin", Filter = "License Files (*.bin)|*.bin" };

			bool? result = fileDialog.ShowDialog();

			if ( result == true )
				LicenseFilePath = fileDialog.FileName;
		}

		void InstallLicenseFile()
		{
			LastInstallSucceeded = false;			
			var license = File.ReadAllBytes( LicenseFilePath );
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

		bool CanInstallLicenseFile()
		{
			if ( string.IsNullOrEmpty( LicenseFilePath ) )
				return false;
			var fileInfo = new FileInfo( LicenseFilePath );
			return fileInfo.Exists;
		}

		bool IsActivationKeyWellFormed()
		{
			return SpAgent.Product.Activation.IsWellFormedKey( OfflineActivationKey );
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
				if ( columnName == "OfflineActivationKey" )
				{
					if ( string.IsNullOrEmpty( OfflineActivationKey ) )
						result = "Please enter an activation key";
					else if ( OfflineActivationKey.Length != ActivationKeyRequiredLength )
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