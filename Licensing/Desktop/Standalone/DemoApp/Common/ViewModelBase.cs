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

namespace DemoApp.Common
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public IDisplayState DisplayState { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;
		public RelayCommand CloseCommand { get; set; }

		public ViewModelBase()
		{
			CloseCommand = new RelayCommand( () => DisplayState.Exit() );
		}

		protected bool SetProperty<T>( ref T storage, T value, String propertyName = null )
		{
			if ( object.Equals( storage, value ) ) 
				return false;

			storage = value;
			OnPropertyChanged( propertyName );
			return true;
		}

		protected void OnPropertyChanged( string propertyName = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
}