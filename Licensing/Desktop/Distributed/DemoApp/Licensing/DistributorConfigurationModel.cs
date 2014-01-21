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

namespace DemoApp.Licensing
{
	class DistributorConfigurationModel : INotifyPropertyChanged, IDataErrorInfo
	{
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
					OnPropertyChanged( "HasValidDistributorUrl" );
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

		public bool IsValidModel
		{
			get
			{
				return Error == null;
			}
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

	static class DistributorConfigurationModelRepository
	{
		public static DistributorConfigurationModel Load()
		{
			var urlFromConfig = SpAgent.Configuration.DistributorBaseUri;
			return new DistributorConfigurationModel { DistributorUrl = urlFromConfig != null ? urlFromConfig.ToString() : string.Empty };
		}

		public static void Save( DistributorConfigurationModel model )
		{
			var urlToWrite = model.HasValidDistributorUrl ? new Uri( model.DistributorUrl ) : null;
			SpAgent.Configuration.DistributorBaseUri = urlToWrite;
		}
	}
}
