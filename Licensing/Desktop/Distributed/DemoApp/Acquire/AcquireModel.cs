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

		public AcquireModel()
		{
			AcquireCommand = new RelayCommand( Acquire );

			// NB - RunFeatureCommand in this model is only available if a given feature is already held in current context.
			// If RunFeatureCommand isn't available for a given feature, the respective 'Feature X' button bound to this command will be disabled. 
			RunFeatureCommand = new RelayCommand<string>( RunFeature, CanRunFeature, Convert.ToString );
		}

		bool CanRunFeature( string featureName )
		{
			// Checks whether a given feature is held in current context (Distributed or Local)
			return SpAgent.Distributed.Features.Contains( featureName ) || SpAgent.Product.LocalFeatures.ValidContains( featureName );
		}

		void Acquire()
		{
			try
			{
				SpAgent.Distributed.Acquire( x =>
				{
					// Acquire the first available set
					var selectedSet = x.First();
					if ( selectedSet.IsEmpty() )
						DisplayState.NotifyUser( "No features can been acquired. Please check your Licensing Status." );
					return selectedSet;
				} );
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

	static class SetExtensions
	{
		public static bool IsEmpty<T>( this ISet<T> that )
		{
			return !that.Any();
		}
	}
}