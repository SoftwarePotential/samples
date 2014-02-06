using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Diagnostics
{
	public static class CopyrightBanner
	{
		public static string ForUtilityInAssembly( string description, Assembly assembly )
		{
			var fileVersionInfo = FileVersionInfo.GetVersionInfo( assembly.Location );
			string utilityName = String.Format( @"{0} Version {1}",
				description,
				VersionAndBittedness( fileVersionInfo ) );
			return utilityName + Environment.NewLine + CopyrightMessage.Format( fileVersionInfo );
		}

		static string VersionAndBittedness( FileVersionInfo fileVersionInfo )
		{
			return fileVersionInfo.FileVersion + " on " + IntPtr.Size * 8 + @"-bit .NET " + Environment.Version;
		}

		public class CopyrightMessage
		{
			public static string Format( FileVersionInfo fileVersionInfo )
			{
				return "Copyright (C) " + fileVersionInfo.LegalCopyright + " " + fileVersionInfo.CompanyName + ". All rights reserved.";
			}
		}

		public class FileVersionHelpers
		{
			public static FileVersionInfo FileVersionInfoOf( Assembly assembly )
			{
				return FileVersionInfo.GetVersionInfo( assembly.Location );
			}

			public static string VersionStringOf( FileVersionInfo fileVersionInfo )
			{
				return fileVersionInfo.FileVersion.ToString();
			}
		}
	}
}