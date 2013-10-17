using System;
using System.IO;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		static partial void ConfigureLocalStorePath( string rootPath, string vendor, string product, string version, Action<string, string> overridePath )
		{
			//NB - in this MVC sample we are using application-relative App_Data\Licenses folder as license store root path
			overridePath( Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Licenses" ), Path.Combine( product, version ) );
		}
	}
}