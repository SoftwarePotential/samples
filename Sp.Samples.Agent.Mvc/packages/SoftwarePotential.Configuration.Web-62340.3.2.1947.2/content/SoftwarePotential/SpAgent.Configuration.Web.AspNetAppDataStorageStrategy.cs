// NB This file is auto-generated via the SoftwarePotential.Configuration.Web-XXYYY NuGet package.
// For more details see the README at http://docs.softwarepotential.com/Configuration.Web-README.html
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using System;
using System.IO;
using Sp.Agent.Configuration;

namespace Sp.Agent
{
	// TODO doc
	static class AspNetAppDataStorageStrategy
	{
		/// <summary>
		/// <para>Default License Storage Root folder.</para>
		/// </summary>
		/// <value>Yields the absolute path of App_Data\Licenses folder under this web application's base directory (even if this folder doesn't exist).</value>
		public static string AppDataPath
		{
			get { return Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "App_Data" ); }
		}
	}
}
