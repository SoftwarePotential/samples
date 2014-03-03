/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

namespace DemoApp.Checkout
{
	public interface IDisplayCheckoutState
	{
		void ShowAvailableCheckouts();
		void ShowCurrentCheckout();
		void NotifyUser( object message );
		void Close();
	}
}