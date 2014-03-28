/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */
using DemoApp.Activation;
using DemoApp.Common;
using DemoApp.Licenses;
using DemoApp.Properties;
using Sp.Agent;
using System;
using System.ComponentModel;
using System.Windows;

namespace DemoApp.Configuration
{
	public class ConfigurationModel : ViewModelBase, IDataErrorInfo
	{
		public RelayCommand TestConnectionCommand { get; set; }
		public RelayCommand SaveCommand { get; set; }
		public RelayCommand ActivationCommand { get; set; }
		public RelayCommand ViewLicensesCommand { get; set; }
		string _distributorUrl;

		public string DistributorUrl
		{
			get { return _distributorUrl; }
			set
			{
				if ( _distributorUrl != value )
				{
					_distributorUrl = value;
					OnPropertyChanged( "DistributorUrl" );
					TestConnectionCommand.RaiseCanExecuteChanged();
					SaveCommand.RaiseCanExecuteChanged();
				}
			}
		}

		int _licenseCount;
		public int LicenseCount
		{
			get { return _licenseCount; }
			set
			{
				_licenseCount = value;
				OnPropertyChanged( "LicenseCount" );
			}
		}

		public ConfigurationModel()
		{
			TestConnectionCommand = new RelayCommand( TestConnection, HasValidDistributorUrl );
			SaveCommand = new RelayCommand( Save, HasValidDistributorUrlOrEmpty );
			DistributorUrl = DistributorConfigurationRepository.Load();
			LicenseCount = LicenseRepository.LicenseCount( SpAgent.Product );
			ActivationCommand = new RelayCommand( () => DisplayState.Navigate( new ActivationPage() ) );
			ViewLicensesCommand = new RelayCommand( () => DisplayState.Navigate( new LicenseListPage() ) );
		}

		bool HasValidDistributorUrlOrEmpty()
		{
			return HasValidDistributorUrl() || String.Empty == DistributorUrl;
		}

		public bool HasValidDistributorUrl()
		{
			Uri parsedUri;
			bool isWellFormedUri = Uri.TryCreate( DistributorUrl, UriKind.Absolute, out parsedUri );
			return isWellFormedUri && parsedUri.Scheme == Uri.UriSchemeHttp && !parsedUri.IsDefaultPort;
		}

		void TestConnection()
		{
			var diagnosticsResult = DistributorDiagnosticsHelper.GetDiagnosticsInformation( new Uri( DistributorUrl ) );
			MessageBox.Show( diagnosticsResult.GetAllMessagesAsString(), "Connectivity test", MessageBoxButton.OK,
				diagnosticsResult.AllVerificationsPassed ? MessageBoxImage.Information : MessageBoxImage.Warning );
		}

		void Save()
		{
			if ( HasValidDistributorUrl() )
			{
				var diagnosticsResult = DistributorDiagnosticsHelper.GetDiagnosticsInformation( new Uri( DistributorUrl ) );
				if ( !diagnosticsResult.AllVerificationsPassed )
				{
					var messages = diagnosticsResult.GetAllMessagesAsString() + "\nDo you want to save this configuration anyway?";
					if ( !DisplayState.Warn( messages ) )
						return;
				}
			}

			DistributorConfigurationRepository.Save( this );
			SetFirstRunLicensingConfigurationFinishedIfApplies();
		}

		static void SetFirstRunLicensingConfigurationFinishedIfApplies()
		{
			if ( !Settings.Default.FirstRunLicensingConfigurationFinished )
			{
				Settings.Default.FirstRunLicensingConfigurationFinished = true;
				Settings.Default.Save();
			}
		}

		public string this[ string columnName ]
		{
			get
			{
				if ( columnName == "DistributorUrl" )
					if ( !HasValidDistributorUrlOrEmpty() )
						return "Incorrect URL format\nPlease enter full Distributor URL, including the port number (e.g. " + ValidUrlExample + ")";
				return null;
			}
		}

		public string Error
		{
			get { return this[ "DistributorUrl" ]; }
		}

		public const string DistributorUrlPrompt = "Please enter Distributor URL (e.g. " + ValidUrlExample + ")";
		const string ValidUrlExample = "http://licensingserver:8731";
	}

	static class DistributorConfigurationRepository
	{
		public static string Load()
		{
			var urlFromConfig = SpAgent.Configuration.DistributorBaseUri;
			return urlFromConfig != null ? urlFromConfig.ToString() : string.Empty;
		}

		public static void Save( ConfigurationModel model )
		{
			var urlToWrite = model.HasValidDistributorUrl() ? new Uri( model.DistributorUrl ) : null;
			SpAgent.Configuration.DistributorBaseUri = urlToWrite;
		}
	}
}