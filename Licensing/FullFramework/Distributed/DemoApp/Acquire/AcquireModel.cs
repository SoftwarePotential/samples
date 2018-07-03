using System;
using System.Linq;
using System.Windows;
using DemoApp.Common;
using Sp.Agent;
using Sp.Agent.Distributor;

namespace DemoApp.Acquire
{
	class AcquireModel : DemoFeatureRunningModel, IDisposable
	{
		public RelayCommand AcquireCommand { get; private set; }

		public AcquireModel()
		{
			AcquireCommand = new RelayCommand( Acquire );

			// NB - RunFeatureCommand in this model is only available if a given feature is already held in current context.
			// If RunFeatureCommand isn't available for a given feature, the respective 'Feature X' button bound to this command will be disabled. 
			RunFeatureCommand = new RelayCommand<string>( RunFeature, CanRunFeature, Convert.ToString );

			SpAgent.Product.Stores.LicenseInstalled += OnLicenseInstalled;
			SpAgent.Distributed.FeaturesUpdated += OnDistributedFeaturesUpdated;
		}

		bool CanRunFeature( string featureName )
		{
			// Checks whether a given feature is held in current context (Distributed or Local)
			return SpAgent.Distributed.Features.Contains( featureName ) || SpAgent.Product.LocalFeatures.ValidContains( featureName );
		}

		void Acquire()
		{
			if ( SpAgent.Configuration.DistributorBaseUri == null )
			{
				DisplayState.NotifyUser( "There is no distributor server configured. Please configure a server in the configuration dialog." );
				return;
			}
			try
			{
				// Acquire the first available set
				SpAgent.Distributed.Acquire( x => x.First() );
				if ( SpAgent.Distributed.Features.Count == 0 )
					DisplayState.NotifyUser( "No features were available. Please check your Licensing Status." );
			}
			catch ( DistributorRequestException )
			{
				DisplayState.NotifyUser( "There has been an issue contacting your distributor server. Please try again. If the problem persists, please contact your system administrator." );
			}
			catch ( DistributorIntegrityException )
			{
				DisplayState.NotifyUser( "We have detected an integrity issue with your distributor server. Please contact your system administrator." );
			}
		}

		void OnDistributedFeaturesUpdated( object sender, EventArgs e )
		{
			EvaluateRunFeatureCommandAvailability();
		}

		void OnLicenseInstalled( object sender, EventArgs e )
		{
			EvaluateRunFeatureCommandAvailability();
		}

		void EvaluateRunFeatureCommandAvailability()
		{
			// Re-evaluates RunFeature command availability for all buttons bound to this command.
			// All 'Feature X' buttons will get enabled/disabled based on the command availability for a given feature.
			((Window)DisplayState).Dispatcher.BeginInvoke( (Action)(() =>
				RunFeatureCommand.RaiseCanExecuteChanged()) );
		}

		public void Dispose()
		{
			SpAgent.Product.Stores.LicenseInstalled -= OnLicenseInstalled;
			SpAgent.Distributed.FeaturesUpdated -= OnDistributedFeaturesUpdated;
		}
	}
}