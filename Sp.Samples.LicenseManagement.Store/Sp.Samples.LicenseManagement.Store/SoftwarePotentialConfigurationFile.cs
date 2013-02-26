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
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Sp.Samples.LicenseManagement.Store
{
	public class SoftwarePotentialConfigurationFile
	{
		const string SharedSecret = "12345";
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
			//Decrypt password string at this point - AES/Rijndael decrypt function
			string decryptedPassword = Encryption.DecryptString( password, SharedSecret );
			return new Credentials( username, decryptedPassword );
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
			//Encrypting password string before setting the value
			string encryptedPassword = Encryption.EncryptString( password, SharedSecret );
			SetAppSettingsValue( username, "username", appSettings );
			SetAppSettingsValue( encryptedPassword, "password", appSettings );
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
	}

	public static class Encryption
	{
		//For further information on this encryption class, see
		//http://stackoverflow.com/questions/202011/encrypt-decrypt-string-in-net

		static byte[] _salt = Encoding.ASCII.GetBytes( "o4865465sdK5c3" );

		public static string EncryptString( string plainText, string sharedSecret )
		{
			if ( string.IsNullOrEmpty( plainText ) )
				throw new ArgumentNullException( "plainText" );
			if ( string.IsNullOrEmpty( sharedSecret ) )
				throw new ArgumentNullException( "sharedSecret" );
			
			using ( var aesAlg = new RijndaelManaged() )
			{
				//Generate a key from the shared secret and the salt
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes( sharedSecret, _salt );

				aesAlg.Key = key.GetBytes( aesAlg.KeySize / 8 );

				//Create an encryptor to perform the stream transform
				ICryptoTransform encryptor = aesAlg.CreateEncryptor( aesAlg.Key, aesAlg.IV );

				//Create the streams used for encryption
				using ( var msEncrypt = new MemoryStream() )
				{
					//Prepend the Initialization Vector
					msEncrypt.Write( BitConverter.GetBytes( aesAlg.IV.Length ), 0, sizeof( int ) );
					msEncrypt.Write( aesAlg.IV, 0, aesAlg.IV.Length );
					using ( var csEncrypt = new CryptoStream( msEncrypt, encryptor, CryptoStreamMode.Write ) )
					{
						using ( var swEncrypt = new StreamWriter( csEncrypt ) )
							//Write all data to the stream
							swEncrypt.Write( plainText );
						return Convert.ToBase64String( msEncrypt.ToArray() );
					}
				}
			}
		}

		public static string DecryptString( string cipherText, string sharedSecret )
		{
			if ( string.IsNullOrEmpty( cipherText ) )
				throw new ArgumentNullException( "cipherText" );
			if ( string.IsNullOrEmpty( sharedSecret ) )
				throw new ArgumentNullException( "sharedSecret" );
			
			using ( var aesAlg = new RijndaelManaged() )
			{
				//Generate a key from the shared secret and the salt
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes( sharedSecret, _salt );

				//Create streams used for decryption
				byte[] bytes = Convert.FromBase64String( cipherText );
				using ( var msDecrypt = new MemoryStream( bytes ) )
				{
					aesAlg.Key = key.GetBytes( aesAlg.KeySize / 8 );
					//Get the initialization vector from the encrypted stream
					aesAlg.IV = ReadByteArray( msDecrypt );

					//Create a decryptor to perform the stream transform
					ICryptoTransform decryptor = aesAlg.CreateDecryptor( aesAlg.Key, aesAlg.IV );
					using ( var csDecrypt = new CryptoStream( msDecrypt, decryptor, CryptoStreamMode.Read ) )
					using ( var srDecrypt = new StreamReader( csDecrypt ) )
						return srDecrypt.ReadToEnd();
				}
			}
		}
		
		static byte[] ReadByteArray( Stream s )
		{
			byte[] rawLength = new byte[ sizeof( int ) ];
			if ( s.Read( rawLength, 0, rawLength.Length ) != rawLength.Length )
				throw new SystemException( "Stream did not contain properly formatted byte array" );

			byte[] buffer = new byte[ BitConverter.ToInt32( rawLength, 0 ) ];
			if ( s.Read( buffer, 0, buffer.Length ) != buffer.Length )
				throw new SystemException( "Did not read byte array properly" );

			return buffer;
		}
	}
}