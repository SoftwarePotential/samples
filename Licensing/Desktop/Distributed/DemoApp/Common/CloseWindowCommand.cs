/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System.Windows;
using System.Windows.Input;

namespace DemoApp.Common
{
	public class CloseWindowCommand 
	{
		public static readonly ICommand CloseCommand =  new RelayCommand<object>( o => ((Window)o).Close() );
	}
}