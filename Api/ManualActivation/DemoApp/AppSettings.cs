using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DemoApp
{
	static class AppSettings
	{
		public static string ActivationWSUrl
		{
			get { return WebConfigurationManager.AppSettings[ "DemoApp.ManualActivation.ActivationWSUrl" ]; }
		}
	}
}