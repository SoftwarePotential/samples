using System.Windows;
using System.Windows.Input;

namespace Sp.Samples.Agent.WpfApplication.Licenses
{
	/// <summary>
	/// Interaction logic for LicenseList.xaml
	/// </summary>
	public partial class LicenseListView : Window
	{
		public LicenseListView()
		{
			InitializeComponent();
		}

		public static RoutedCommand RemoveLicenseCommand = new RoutedCommand();

		void RemoveLicenseCommand_Executed( object sender, ExecutedRoutedEventArgs e )
		{
			if ( MessageBox.Show( "Are you sure you want to remove this license?", "Please confirm", MessageBoxButton.YesNo ) == MessageBoxResult.Yes )
			{
				//Delete license
				object[] parameters = (object[])e.Parameter;
				var licenseListModel = (LicenseListModel)parameters[ 0 ];
				var licenseToRemove = (LicenseItemModel)parameters[ 1 ];

				licenseListModel.RemoveLicenseItem( licenseToRemove );
			}
		}

		void RemoveLicenseCommand_CanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
	}
}
