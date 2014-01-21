// NB This file is auto-added via the SoftwarePotential.Configuration.Distributor-<ShortCode> NuGet package.
//
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Sp.Agent.Distributor.AppSettingsHelpers
{
	/// <summary>
	/// Encapsulates opening (and/or creating and seeding) of a <c>Sp.Agent.Distributor.dll.config</c> 
	/// file that will be maintained outside of an application's <c>app.config</c> file.
	/// </summary>
	class WritableConfigurationFile
	{
		/// <summary>
		/// The name of the config file. As all programmatic access is to be gated 
		/// through this class, this should not need to be externally known.
		/// </summary>
		const string RuntimeConfigFilename = "Sp.Agent.Distributor.dll.config";

		/// <summary>
		/// Opens the file. If the file is not present, places valid stub content there first.
		/// </summary>
		/// <param name="configDirectory">The folder within which the file is to be maintained. Should have appropriate sharing privileges such that all users under whose control the application may need to be reconfigured will have access to write files within it.</param>
		public static SpAgentWritableDistributorAppSettings OpenOrCreateEmpty( string configDirectory )
		{
			return OpenOrInitializeFromTemplateContent( @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration/>", configDirectory );
		}

		static SpAgentWritableDistributorAppSettings OpenOrInitializeFromTemplateContent( string configFileTemplateContent, string configDirectory )
		{
			return OpenOrSeed( runtimeConfigFile => File.WriteAllText( runtimeConfigFile, configFileTemplateContent, Encoding.UTF8 ), configDirectory );
		}

		static SpAgentWritableDistributorAppSettings OpenOrSeed( Action<string> seedFile, string configDirectory )
		{
			var runtimeConfigFile = new FileInfo( Path.Combine( configDirectory, RuntimeConfigFilename ) );
			if ( !runtimeConfigFile.Exists )
				seedFile( runtimeConfigFile.FullName );
			return Open( runtimeConfigFile );
		}

		/// <summary>
		/// Opens the file. Will fail if the file is not present.
		/// </summary>
		/// <param name="runtimeConfigFile">The folder within which the file is stored.</param>
		public static SpAgentWritableDistributorAppSettings Open( FileInfo runtimeConfigFile )
		{
			return SpAgentDistributorConfiguration.OpenStandaloneFile( runtimeConfigFile );
		}
	}

	/// <summary>
	/// Manages opening a Distributor Configuration file from either app.config or a standalone file.
	/// </summary>
	static class SpAgentDistributorConfiguration
	{
		/// <summary>
		/// Accesses [in read/write mode] the BaseUri setting maintained the specified <paramref name="file" />.
		/// </summary>
		public static SpAgentWritableDistributorAppSettings OpenStandaloneFile( FileInfo file )
		{
			return new SpAgentWritableDistributorAppSettings(
				System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(
					new ExeConfigurationFileMap { ExeConfigFilename = file.FullName },
					ConfigurationUserLevel.None ) );
		}

		/// <summary>
		/// Accesses [in read-only mode] the BaseUri setting maintained externally in the application's <c>app.config</c> file.
		/// </summary>
		public static SpAgentDistributorAppSettings FromAppConfig()
		{
			return new SpAgentDistributorAppSettings( System.Configuration.ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None ) );
		}
	}

	/// <summary>
	/// Manages access to an <c>configuration/appSetting/Sp.Agent.Distributor.BaseUri</c> entry in an Application Configuration file.
	/// </summary>
	/// <remarks>
	/// This class handles the reading aspect only. For writing support, see <see cref="SpAgentWritableDistributorAppSettings"/>.
	/// </remarks>
	class SpAgentDistributorAppSettings
	{
		// NB any compiler errors around here mean you're missing an Assembly reference to System.Configuration.dll
		protected readonly System.Configuration.Configuration _configuration;

		/// <summary>
		/// Creates an instance referring to the supplied <see cref="System.Configuration.Configuration"/>.
		/// </summary>
		public SpAgentDistributorAppSettings( System.Configuration.Configuration configuration )
		{
			_configuration = configuration;
		}

		/// <summary>
		/// The setting name the class uses.
		/// </summary>
		/// <value>
		/// <c>Sp.Agent.Distributor.BaseUri</c>
		/// </value>
		protected static string ConfigSettingName
		{
			get { return "Sp.Agent.Distributor.BaseUri"; }
		}

		/// <summary>
		/// Extracts the Configured Uri. Throws if the data is not a valid Uri.
		/// </summary>
		/// <returns>The configured <see cref="Uri"/> or <c>null</c> if no entry is present.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the setting is not a valid <see cref="Uri"/>.</exception>
		public Uri ReadBaseUriOrDefault()
		{
			var setting = _configuration.AppSettings.Settings[ ConfigSettingName ];
			if ( setting == null )
				return null;
			var settingValue = setting.Value;
			Uri uri;
			if ( !Uri.TryCreate( settingValue, UriKind.Absolute, out uri ) )
				throw new InvalidOperationException( "Malformed Uri in Configuration Setting. Has there been an external modification? (" + ConfigSettingName + " = " + settingValue + " in " + Filepath + ")" );
			return uri;
		}

		/// <summary>
		/// The absolute path of the Configuration file we are reading from. 
		/// </summary>
		/// <remarks>
		/// (works regardless of whether we are a .exe.config or a standalone file)
		/// </remarks>
		string Filepath
		{
			get { return _configuration.FilePath; }
		}
	}

	/// <summary>
	/// Manages reading or writing a <c>configuration/appSetting/Sp.Agent.Distributor.BaseUri</c> entry in an Application Configuration file.
	/// </summary>
	/// <remarks>
	/// This class handles the writing aspect (the base class, <see cref="SpAgentDistributorAppSettings"/> handles the reading).
	/// </remarks>
	class SpAgentWritableDistributorAppSettings : SpAgentDistributorAppSettings
	{
		/// <summary>
		/// Creates an instance. The supplied <paramref name="configuration"/> must be writable.
		/// </summary>
		/// <param name="configuration">Configuration. Must be writable.</param>
		public SpAgentWritableDistributorAppSettings( System.Configuration.Configuration configuration )
			: base( configuration )
		{
		}

		/// <summary>
		/// Updates the setting in the associated <see cref="System.Configuration.Configuration"/>.
		/// </summary>
		public void UpdateBaseUri( Uri value )
		{
			if ( value == null )
				throw new ArgumentNullException( "value" );

			_configuration.AppSettings.Settings.Remove( ConfigSettingName );
			_configuration.AppSettings.Settings.Add( ConfigSettingName, value.ToString() );
			_configuration.Save();
		}
	}

	/// <summary>
	/// Caches the (non-<c>null</c> value of writable property. All <c>set</c> operations are expected to be 
	/// passed through the cache too in order to ensure the <c>get</c> will yield a consistent result.
	/// </summary>
	/// <typeparam name="T">The type of value to be cached.</typeparam>
	class WriteThroughCachedValue<T> where T : class
	{
		readonly Action<T> _set;
		readonly Func<T> _get;
		T _cachedValue;

		/// <summary>
		/// Initializes the caching object given a pair of reading/writing delegates.
		/// </summary>
		public WriteThroughCachedValue( Action<T> update, Func<T> retrieve )
		{
			if ( update == null )
				throw new ArgumentNullException( "update" );
			if ( retrieve == null )
				throw new ArgumentNullException( "retrieve" );

			_set = update;
			_get = retrieve;
		}

		/// <summary>
		/// The value being managed.
		/// </summary>
		public T Value
		{
			get
			{
				if ( _cachedValue == null )
					_cachedValue = _get();
				return _cachedValue;
			}
			set
			{
				_set( value );
				_cachedValue = value;
			}
		}
	}
}