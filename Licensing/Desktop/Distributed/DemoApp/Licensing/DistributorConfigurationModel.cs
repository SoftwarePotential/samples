/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.ComponentModel;
using Sp.Agent;
using DemoApp.Common;
using System.Windows;
using DemoApp.Properties;

namespace DemoApp.Licensing
{
	class DistributorConfigurationModel : INotifyPropertyChanged, IDataErrorInfo
	{
		public RelayCommand TestConnectionCommand { get; set; }
		public RelayCommand SaveCommand { get; set; }
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

		public bool HasValidDistributorUrl
		{
			get
			{
				Uri parsedUri;
				bool isWellFormedUri = Uri.TryCreate( DistributorUrl, UriKind.Absolute, out parsedUri );
				return isWellFormedUri && parsedUri.Scheme == Uri.UriSchemeHttp && !parsedUri.IsDefaultPort;
			}
		}

		public DistributorConfigurationModel()
		{
			TestConnectionCommand = new RelayCommand( TestConnection, CanTestConnection );
			SaveCommand = new RelayCommand( Save, CanSave );
			DistributorUrl = DistributorConfigurationRepository.Load();
		}

		void TestConnection()
		{
			var diagnosticsResult = DistributorDiagnosticsHelper.GetDiagnosticsInformation( new Uri( DistributorUrl ) );
			MessageBox.Show( diagnosticsResult.GetAllMessagesAsString(), "Connectivity test", MessageBoxButton.OK,
				diagnosticsResult.AllVerificationsPassed ? MessageBoxImage.Information : MessageBoxImage.Warning );
		}

		bool CanTestConnection()
		{
			return HasValidDistributorUrl;
		}

		void Save()
		{
			if ( HasValidDistributorUrl )
			{
				var diagnosticsResult = DistributorDiagnosticsHelper.GetDiagnosticsInformation( new Uri( DistributorUrl ) );
				if ( !diagnosticsResult.AllVerificationsPassed )
				{
					var messages = diagnosticsResult.GetAllMessagesAsString() + "\nDo you want to save this configuration anyway?";

					var warningMessageBoxResult = MessageBox.Show( messages, "Warning", MessageBoxButton.YesNo,
						MessageBoxImage.Warning );
					if ( warningMessageBoxResult == MessageBoxResult.No )
						return;
				}
			}

			DistributorConfigurationRepository.Save( this );
			SetFistRunLicensingConfigurationFinishedIfApplies();
		//	Close();
		}

		static void SetFistRunLicensingConfigurationFinishedIfApplies()
		{
			if ( !Settings.Default.FirstRunLicensingConfigurationFinished )
			{
				Settings.Default.FirstRunLicensingConfigurationFinished = true;
				Settings.Default.Save();
			}
		}

		bool CanSave()
		{
			return HasValidDistributorUrl;
		}

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

		#region IDataErrorInfo Members
		public string this[ string columnName ]
		{
			get
			{
				string result = null;
				if ( columnName == "DistributorUrl" )
				{
					if ( !string.IsNullOrEmpty( DistributorUrl ) )
					{
						if ( !HasValidDistributorUrl )
							result = "Incorrect URL format\nPlease enter full Distributor URL, including the port number (e.g. " + ValidUrlExample + ")";
					}
				}
				return result;
			}
		}

		public string Error
		{
			get { return this[ "DistributorUrl" ]; }
		}
		#endregion

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

		public static void Save( DistributorConfigurationModel model )
		{
			var urlToWrite = model.HasValidDistributorUrl ? new Uri( model.DistributorUrl ) : null;
			SpAgent.Configuration.DistributorBaseUri = urlToWrite;
		}
	}
}
