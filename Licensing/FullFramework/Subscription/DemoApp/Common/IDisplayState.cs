/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

namespace DemoApp.Common
{
	public interface IDisplayState
	{
		void NotifyUser( object message );
		bool Warn( object message );
		void Exit();
	}
}