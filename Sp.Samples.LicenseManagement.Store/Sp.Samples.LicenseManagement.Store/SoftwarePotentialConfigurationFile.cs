/*
 * Copyright 2013 (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */

// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

using System;
using System.Linq;
using System.Xml.Linq;

namespace Sp.Samples.LicenseManagement.Store
{
	public class SoftwarePotentialConfigurationFile
	{
		readonly string _path;

		public SoftwarePotentialConfigurationFile( string path )
		{
			_path = path;
		}

		public Credentials ReadCredentials()
		{
			var configurationNode = XElement.Load( _path );
			var appSettings = configurationNode.Element( "appSettings" );
			string username = GetAppSettingsValueOrDefault( "username", appSettings );
			string password = GetAppSettingsValueOrDefault( "password", appSettings );
			if ( String.IsNullOrEmpty( username ) || password == null )
				throw new CredentialsNotConfiguredException();
			return new Credentials( username, password );
		}

		// TOCONSIDER: Move top a higher level wrapper class which also supplies client proxies etc.
		public void VerifyCredentialsAreConfigured()
		{
			ReadCredentials();
		}

		static string GetAppSettingsValueOrDefault( string settingName, XElement appSettingsElement )
		{
			XAttribute valueAttributeForSetting = ValueAttributeForSetting( settingName, appSettingsElement );
			if ( valueAttributeForSetting == null )
				return null;
			return valueAttributeForSetting.Value;
		}

		public void WriteCredentials( string username, string password )
		{
			var configurationNode = XElement.Load( _path, LoadOptions.PreserveWhitespace );
			var appSettings = configurationNode.Element( "appSettings" );
			SetAppSettingsValue( username, "username", appSettings );
			SetAppSettingsValue( password, "password", appSettings );
			configurationNode.Save( _path );
		}

		static void SetAppSettingsValue( string value, string settingName, XElement appSettingsElement )
		{
			var key = SettingKey( settingName );
			try
			{
				XElement appSettingAddElement = AppSettingAddElementKeyedBy( key, appSettingsElement );
				appSettingAddElement.SetAttributeValue( "value", value );
			}
			catch ( Exception ex )
			{
				throw new Exception( String.Format( "There has been a problem setting the 'value' associated with the configuration/appSettings/add with key ='{0}'. Please ensure settings are specified in standard .NET appSettings format.", key ), ex );
			}
		}

		static XAttribute ValueAttributeForSetting( string settingName, XElement appSettingsElement )
		{
			var key = SettingKey( settingName );
			try
			{
				return AppSettingAddElementKeyedBy( key, appSettingsElement ).Attribute( "value" );
			}
			catch ( Exception ex )
			{
				throw new Exception( String.Format( "There has been a problem accessing the 'value' associated with the configuration/appSettings/add with key ='{0}'. Please ensure settings are specified in standard .NET appSettings format.", key ), ex );
			}
		}

		static XElement AppSettingAddElementKeyedBy( string key, XElement appSettingsElement )
		{
			return appSettingsElement.Elements().Single( x => x.Name == "add" && x.Attribute( "key" ).Value == key );
		}

		internal static string SettingKey( string settingName )
		{
			return "SoftwarePotential.LicenseManagement.Credentials." + settingName;
		}

		public class Credentials
		{
			public Credentials( string username, string password )
			{
				Username = username;
				Password = password;
			}

			public string Username { get; set; }
			public string Password { get; set; }
		}
	}

	public class CredentialsNotConfiguredException : Exception
	{
		public CredentialsNotConfiguredException()
			: base( FormatMessage() )
		{
		}

		static string FormatMessage()
		{
			return String.Format( @"The Software Potential API credentials (appSettings values for {0} and/or {1}) have not been initialized).
Please ensure the credentials are provisioned correctly", SoftwarePotentialConfigurationFile.SettingKey( "username" ), SoftwarePotentialConfigurationFile.SettingKey( "password" ) );
		}
	}
}