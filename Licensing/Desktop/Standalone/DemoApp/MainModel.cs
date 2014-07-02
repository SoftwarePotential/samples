using DemoApp.Common;
using Sp.Agent;

namespace DemoApp
{
	public class MainModel : ViewModelBase
	{
		public RelayCommand Feature1Command { get; set; }
		public RelayCommand Feature2Command { get; set; }

		public MainModel()
		{
			Feature1Command = new RelayCommand( Feature1, CanExecuteFeature1 );
			Feature2Command = new RelayCommand( Feature2, CanExecuteFeature2 );
		}

		[Demo_10.Features.Feature1]
		void Feature1()
		{
			DisplayState.NotifyUser( "Feature 1 accessed successfully" );
		}

		bool CanExecuteFeature1()
		{
			return SpAgent.Product.LocalFeatures.ValidContains( Demo_10.Features.Feature1.Name );
		}

		[Demo_10.Features.Feature2]
		void Feature2()
		{
			DisplayState.NotifyUser( "Feature 2 accessed successfully" );
		}

		bool CanExecuteFeature2()
		{
			return SpAgent.Product.LocalFeatures.ValidContains( Demo_10.Features.Feature2.Name );
		}
	}
}
