using System;
using System.Collections.Generic;
using System.Linq;
using DemoApp.Common;
using Sp.Agent;
using Sp.Agent.Distributor;

namespace DemoApp.Acquire
{
	class AcquireModel : DemoFeatureRunningModel
	{
		public RelayCommand AcquireCommand { get; private set; }
		ISet<string> _featuresHeld = new HashSet<string>();

		public AcquireModel()
		{
			_featuresHeld = SpAgent.Distributed.Features;
			AcquireCommand = new RelayCommand( Acquire );

			// NB - RunFeatureCommand in this model is only available if a given feature is already held in current Distributed Context.
			// If RunFeatureCommand isn't available for a given feature, the respective 'Feature X' button bound to this command will be disabled. 
			RunFeatureCommand = new RelayCommand<int>( RunFeature, CanRunFeature, Convert.ToInt32 );
		}

		bool CanRunFeature( int featureNumber )
		{
			return _featuresHeld.Contains( "Feature" + featureNumber );
		}

		void Acquire()
		{
			try
			{
				SpAgent.Distributed.Acquire( x =>
				{
					if ( !x.Any() )
					{
						DisplayState.NotifyUser( "No features can been acquired. Please check your Licensing Status." );
						return new string[ 0 ];
					}
					// Acquire the first available set
					return x.First();
				} );
				_featuresHeld = SpAgent.Distributed.Features;
			}
			catch ( DistributorRequestException )
			{
				DisplayState.NotifyUser( "There has been an issue contacting your distributor server. Please try again. If the problem persists, please contact your system administrator." );
			}
			catch ( DistributorIntegrityException )
			{
				DisplayState.NotifyUser( "We have detected an integrity issue with your distributor server. Please contact your system administartor." );
			}
			catch ( NoDistributorException )
			{
				DisplayState.NotifyUser( "There is no distributor server configured. Please configure a server in the configuration dialog." );
			}
			finally
			{
				// Re-evaluates RunFeature command availability for all buttons bound to this command.
				// All 'Feature X' buttons will get enabled/disabled based on the command availability for a given feature.
				RunFeatureCommand.RaiseCanExecuteChanged();
			}
		}
	}
}