// NB This file is auto-generated via the SoftwarePotential.Configuration.Local NuGet packages.
// 
// CONSIDER RENAMING OR MOVING THIS FILE SO A PACKAGE UPDATE CANNOT UNDO ANY CHANGES YOU MAKE

using System;
using System.IO;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		// TODO doc
		public static string ConfiguredExternallyManagedRootPath()
		{
			var paths = ConfiguredBaseAndRelativePathElements();
			return Path.Combine( paths.Item1, paths.Item2 );
		}

		public static Tuple<string, string> ConfiguredBaseAndRelativePathElements()
		{
			var basePath = default(string);
			ConfigureStorageBasePath( value => basePath = value );
			if ( basePath == null )
				throw new InvalidOperationException(); // TODO TP 1676 message
			var relativePath = default(string);
			ConfigureStorageRelativePath( SpProduct.Vendor, SpProduct.Name, SpProduct.Version, value => relativePath = value );
			if ( relativePath == null )
				throw new InvalidOperationException(); // TODO TP 1676 message
			return Tuple.Create( basePath, relativePath );
		}

		static partial void ConfigureStorageBasePath( Action<string> configure );
		static partial void ConfigureStorageRelativePath( string vendor, string product, string version, Action<string> configure );
	}
}

