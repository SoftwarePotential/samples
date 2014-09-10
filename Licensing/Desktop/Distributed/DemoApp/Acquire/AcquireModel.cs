using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DemoApp.BusinessLogic;
using DemoApp.Common;
using Sp.Agent;
using Sp.Agent.Distributor;

namespace DemoApp.Acquire
{
	class AcquireModel : ViewModelBase
	{
		public RelayCommand<int> RunFeatureCommand { get; private set; }
		public RelayCommand AcquireCommand { get; private set; }
		ISet<string> _featuresHeld = new HashSet<string>();

		public AcquireModel()
		{
			_featuresHeld = SpAgent.Distributed.Features;
			RunFeatureCommand = new RelayCommand<int>( RunFeature, CanRunFeature, Convert.ToInt32 );
			AcquireCommand = new RelayCommand( AcquireFirstPool );
		}

		void AcquireFirstPool()
		{
			try
			{
				SpAgent.Distributed.Acquire( x =>
				{
					var firstAvailablePool = x.FirstOrDefault();
					return firstAvailablePool != null ? firstAvailablePool.ToArray() : new string[ 0 ];
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
				RunFeatureCommand.RaiseCanExecuteChanged();
			}
			if ( _featuresHeld.Count == 0 )
				DisplayState.NotifyUser( "No features have been acquired. Please check your Licensing Status." );
		}

		void RunFeature( int featureNumber )
		{
			switch ( featureNumber )
			{
				case 1: MyAlgorithms.AccessFeature1();
					break;
				case 2: MyAlgorithms.AccessFeature2();
					break;
				case 3: MyAlgorithms.AccessFeature3();
					break;
				default:
					throw new ArgumentOutOfRangeException( "featureNumber" );
			}
			LastSuccessfulFeatureExecutionMessage = string.Format( "Feature {0} accessed successfully", featureNumber );
		}

		bool CanRunFeature( int featureNumber )
		{
			return _featuresHeld.Contains( "Feature" + featureNumber );
		}

		string _lastSuccessfulFeatureExecutionMessage;

		public string LastSuccessfulFeatureExecutionMessage
		{
			get { return _lastSuccessfulFeatureExecutionMessage; }
			set
			{
				_lastSuccessfulFeatureExecutionMessage = value;
				OnPropertyChanged( "LastSuccessfulFeatureExecutionMessage" );
			}
		}
	}
}