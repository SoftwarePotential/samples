/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.Windows.Controls;

namespace DemoApp.Acquire
{
	/// <summary>
	/// Interaction logic for AcquirePage.xaml
	/// </summary>
	public partial class AcquirePage : Page
	{
		public AcquirePage()
		{
			InitializeComponent();

			// NB - AcquireModel subscribes to some SpAgent events; the event handlers in that model need to be unsubscribed when the page gets unloaded
			Unloaded += ( s, e ) => DisposeViewModel();
		}

		void DisposeViewModel()
		{
			((IDisposable)DataContext).Dispose();
		}
	}
}
