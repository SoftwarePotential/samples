/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using DemoApp.BusinessLogic;
using DemoApp.Common;

namespace DemoApp
{
	class DemoFeatureRunningModel : ViewModelBase
	{
		public RelayCommand<string> RunFeatureCommand { get; set; }

		public DemoFeatureRunningModel()
		{
			RunFeatureCommand = new RelayCommand<string>( RunFeature, _ => true, Convert.ToString );
		}

		public void RunFeature( string featureName )
		{
			switch ( featureName )
			{
				case "Feature1": MyAlgorithms.AccessFeature1();
					break;
				case "Feature2": MyAlgorithms.AccessFeature2();
					break;
				case "Feature3": MyAlgorithms.AccessFeature3();
					break;
				default:
					throw new ArgumentOutOfRangeException( "featureName" );
			}
			LastSuccessfulFeatureExecutionMessage = string.Format( "{0} accessed successfully", featureName );
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