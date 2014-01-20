using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoApp.Models
{
	public class ActivationRequestHelper
	{
		const string BEGIN_REQUEST = "--BEGIN-REQUEST--";
		const string END_REQUEST = "--END-REQUEST--";
		public static byte[] ExtractRequestBlob( string activationRequest )
		{
			activationRequest = activationRequest.Trim();
			activationRequest = activationRequest.Remove( 0, BEGIN_REQUEST.Length );
			int endRequestIndex = activationRequest.IndexOf( END_REQUEST );
			activationRequest = activationRequest.Remove( endRequestIndex, END_REQUEST.Length );
			return Convert.FromBase64String( activationRequest );
		}
	}
}