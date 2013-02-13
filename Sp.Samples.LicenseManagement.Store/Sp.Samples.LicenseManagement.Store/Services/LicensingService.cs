using Sp.Samples.LicenseManagement.Store.LicenseManagementWS;
using System.Configuration;
using System.ServiceModel;
using System.Web.Configuration;

namespace Sp.Samples.LicenseManagement.Store.Services
{
	//public class LicensingService
	//{
	//	public static LicenseManagerSection _Config = ConfigurationManager.GetSection( "licenseManager" ) as LicenseManagerSection;
	//	public static UserCredentials GetCredentials()
	//	{
	//		string uName = string.Empty;
	//		string pWord = string.Empty;

	//		foreach ( SpCredentialElement spCredentialElement in _Config.SpCredentials )
	//		{
	//			uName = spCredentialElement.Username;
	//			pWord = spCredentialElement.Password;				
	//		}
	//		UserCredentials userCredentials = new UserCredentials( uName, pWord );

	//		return userCredentials;
	//	}
	//	#region Licensing
	//	public static License CreateLicenseFromSkuId( string SkuId )
	//	{			
	//		//UserCredentials ApiCredentials = new UserCredentials( _Config.Username, _Config.Password );

	//		LicenseManagementWSClient client = CreateLicenseManagementClient( GetCredentials(  ) );
	//		try
	//		{
	//			LicenseInfo licenseInfo = client.GetSkuById( SkuId ).LicenseInfo;

	//			//Issue License from License info
	//			License license = client.CreateLicense( licenseInfo );
	//			client.Close();
	//			return license;
	//		}
	//		catch ( FaultException apiEx )
	//		{
	//			client.Abort();
	//			throw apiEx;
	//		}
	//	}

	//	private static LicenseManagementWSClient CreateLicenseManagementClient( UserCredentials Credentials )
	//	{
	//		//System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(Validator);

	//		LicenseManagementWSClient client = new LicenseManagementWSClient(
	//			"WSHttpBinding_ILicenseManagementWS", "https://srv.softwarepotential.com/SLMServerWS/LicenseManagementWS.svc" );
	//		client.ClientCredentials.UserName.UserName = Credentials.username;
	//		client.ClientCredentials.UserName.Password = Credentials.password;

	//		return client;
	//	}
	//	#endregion
	//}

	//public class UserCredentials
	//{
	//	string _username;
	//	string _password;
	//	public string username
	//	{
	//		get
	//		{
	//			return _username;
	//		}
	//		set
	//		{
	//			_username = username;
	//		}
	//	}
	//	public string password
	//	{
	//		get
	//		{
	//			return _password;
	//		}
	//		set
	//		{
	//			_password = password;
	//		}
	//	}

	//	public UserCredentials( string username, string password )
	//	{
	//		_username = username;
	//		_password = password;
	//	}
	//}

	//public class SpCredentialElement : ConfigurationElement
	//{
	//	[ConfigurationProperty( "username", IsKey = true, IsRequired = true )]
	//	public string Username
	//	{
	//		get { return (string)this[ "username" ]; }
	//		set { this[ "username" ] = value; }
	//	}

	//	[ConfigurationProperty( "password", IsRequired = true )]
	//	public string Password
	//	{
	//		get { return (string)this[ "password" ]; }
	//		set { this[ "password" ] = value; }
	//	}
	//}

	//[ ConfigurationCollection( typeof( SpCredentialElement ) ) ]
	//public class SpCredentialElementCollection : ConfigurationElementCollection
	//{
	//	protected override ConfigurationElement CreateNewElement()
	//	{
	//		return new SpCredentialElement();
	//	}
	//	protected override object GetElementKey( ConfigurationElement element )
	//	{
	//		return ((SpCredentialElement)element).Username;
	//	}
	//}

	//public class LicenseManagerSection : ConfigurationSection
	//{
	//	[ConfigurationProperty( "spCredentials", IsDefaultCollection = true )]
	//	public SpCredentialElementCollection SpCredentials
	//	{
	//		get { return ( SpCredentialElementCollection )this[ "spCredentials" ]; }
	//		set { this[ "spCredentials" ] = value; }
	//	}
	//}
}