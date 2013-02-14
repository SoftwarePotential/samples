using System.ComponentModel.DataAnnotations;

namespace Sp.Samples.LicenseManagement.Store.Models
{
	public class CredentialsModel
	{
		[Required( ErrorMessage="Username is required!" )]
		[Display( Name = "Software Potential Username" )]
		public string Username { get; set; }
		[Required( ErrorMessage="Password is required!" )]
		[ DataType( DataType.Password ) ]
		[ Display( Name="Software Potential Password" ) ]
		public string Password { get; set; }
	}
}

