// NB This file is auto-generated via the SoftwarePotential.Configuration.Local.SingleUser-XXYYY NuGet package.
// For more details see the README at http://docs.softwarepotential.com/Configuration.Local.SingleUser-README.html
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using Sp.Agent.Configuration;
using System;
using System.IO;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		/// <summary>
		/// <para>Configures licenses to be stored under <c>C:\Users\USERNAME\AppData\Local\MyCompany\MyProduct\MyVersion</c>.</para>
		/// <para>NB THIS LOCATION IS SPECIFIC TO A USER PROFILE AND WILL MEAN YOUR CUSTOMERS CAN NOT SHARE LICENSES ACROSS PROFILES.</para>
		/// <para>See <c>SoftwarePotential.Configuration.Local.MultiUser</c> if you need to address this restriction [by having an installer create a shared area]</para>
		/// </summary>
		/// /// <param name="agentContext">The context, emanating from your Sp.Agent package.</param>
		/// <remarks>
		/// <para>Slots into the extension point exposed by the SoftwarePotential.Configuration package in its SpAgent.cs file.</para>
		/// </remarks>
		static partial void ConfigureAgent( IAgentContext agentContext )
		{
			var location = LocalSingleUserStorePath;
			var rootPath = location.Item1;
			var relativePath = location.Item2;
			agentContext.Configure( configure => configure
				.WithSingleUserStore( rootPath, relativePath )
				.CompleteWithDefaults() );
		}

		/// <summary>
		/// <para>Computes the (absolute and relative) path to use for license storage.</para>
		/// <para>See ConfigureLocalSingleUserStorePath() for a mechanism to override this cleanly.</para>
		/// </summary>
		static Tuple<string, string> LocalSingleUserStorePath
		{
			get
			{
				var baseDir = LocalApplicationDataFolderPath;
				var vendor = SpProduct.Vendor;
				var product = SpProduct.Name;
				var version = SpProduct.Version;
				var overridePath = default( Tuple<string, string> );

				// Call the partial method. If it is implemented, it will call us back via the lambda and the result will be stashed in overridePath
				ConfigureLocalStorePath(
					baseDir, vendor, product, version,
					( rootPath, relativePath ) => overridePath = Tuple.Create( rootPath, relativePath ) );
				return overridePath ?? Tuple.Create( baseDir, Path.Combine( SpProduct.Vendor, SpProduct.Name, SpProduct.Version ) );
			}
		}

		/// <summary>
		/// <para>Placeholder to enable overriding the store base path (without requiring the editing of this file if desired).</para>
		/// <para>See SpAgent.Configuration.Local.Customizations.cs for a mechanism to override this cleanly.</para>
		/// </summary>
		/// <param name="rootPath">The absolute path to application data folder (e.g. C:\Users\USERNAME\AppData\Local).</param>
		/// <param name="vendor">Vendor name</param>
		/// <param name="product">Product name</param>
		/// <param name="version">Product version</param>
		/// <param name="overridePath">Action that needs to be called to supply the customized store path</param>
		static partial void ConfigureLocalStorePath( string rootPath, string vendor, string product, string version, Action<string, string> overridePath );

		/// <summary>
		/// <para>Default License Storage Root folder.</para>
		/// <para>See ConfigureLocalSingleUserStorePath() for a mechanism to override this cleanly.</para>
		/// </summary>
		/// <value>Yields the ProgramData path for this machine (even if that folder does not exist).</value>
		static string LocalApplicationDataFolderPath
		{
			get
			{
				return Environment.GetFolderPath(
					Environment.SpecialFolder.LocalApplicationData, // %LOCALAPPDATA%, typically expands to C:\Users\USERNAME\AppData\Local
					Environment.SpecialFolderOption.DoNotVerify ); // If it can't be located, we still want a path (.None by default substitutes string.Empty if the folder is not found)
			}
		}
	}
}