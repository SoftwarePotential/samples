using DemoApp.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace DemoApp
{
	public static class ActivationExceptionHelper
	{
		public static string TranslateExceptionToMessage( Exception antecedentException )
		{
			var ex = antecedentException as FaultException;
			if ( ex != null )
				return TranslateActivationFaultCodeString( antecedentException, ex.Code.Name );
			throw antecedentException;
		}

		static string TranslateActivationFaultCodeString( Exception antecedentException, string faultCodeString )
		{
			if ( faultCodeString == ActivationError.LicenseExpired.ToString() )
				return "The License associated with the provided key has Expired.";
			else if ( faultCodeString == ActivationError.NumberOfActivationsExceeded.ToString() )
				return "Maximum number of activations exceeded for the provided key.";
			else if ( faultCodeString == ActivationError.LicenseDisabled.ToString() )
				return "The License associated with the provided key is Disabled.";
			else if ( faultCodeString == ActivationError.DeviceChanged.ToString() )
				return "The license associated with the provided key is a Single Machine License. Reactivations of this license are only permitted on the machine on which it was first activated.";
			else if ( faultCodeString == ActivationError.TagsChanged.ToString() )
				return "The license being activated has unmatched Custom Tag constraints. Reactivations of this license are only permitted on matching machine environments.";
			else
				return "Activation Server Exception occurred with Fault Code: " + faultCodeString + ".";			
		}
	}
}