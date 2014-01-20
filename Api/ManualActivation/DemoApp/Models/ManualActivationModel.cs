using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DemoApp.Models
{
	public class ManualActivationModel
	{
		[Required, StringLength( 500, MinimumLength = 1 )]
		[ValidateActivationRequest]
		public string ActivationRequest { get; set; }

		public Guid FileId { get; set; }
	}

	class ValidateActivationRequest : ValidationAttribute
	{

		public override bool IsValid( object value )
		{
			return Regex.IsMatch( value.ToString(), "--BEGIN-REQUEST--[\\w\\W]*--END-REQUEST--" );
		}

		public override string FormatErrorMessage( string name )
		{
			return String.Format( CultureInfo.CurrentUICulture, "Invalid manual activation request", name );
		}
	}
}