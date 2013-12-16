using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Sp.Agent;
using Resource = Sp.Samples.Agent.Mvc.App_LocalResources;

namespace Sp.Samples.Agent.Mvc.Models
{
	public class ActivationModel
	{
		public const int ActivationKeyRequiredLength = 29;

		[Required( ErrorMessageResourceName = "ActivationKeyRequiredErrorMessage", ErrorMessageResourceType = typeof( Resource.Models.ActivationModels ) )]
		[StringLength( ActivationKeyRequiredLength, MinimumLength = ActivationKeyRequiredLength, ErrorMessageResourceName = "ActivationKeyInvalidLengthErrorMessage", ErrorMessageResourceType = typeof( Resource.Models.ActivationModels ) )]
		[ValidateActivationKey]
		[Display( Name = "ActivationKeyLabel", ResourceType = typeof( Resource.Models.ActivationModels ) )]
		public string ActivationKey { get; set; }

		[Required]
		public Guid PostToken { get; set; }

		[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
		sealed class ValidateActivationKeyAttribute : ValidationAttribute
		{
			public ValidateActivationKeyAttribute()
			{
				ErrorMessageResourceType = typeof( Resource.Models.ActivationModels );
				ErrorMessageResourceName = "ActivationKeyInvalidFormatErrorMessage";
			}

			public override string FormatErrorMessage( string name )
			{
				return String.Format( CultureInfo.CurrentUICulture, ErrorMessageString, name );
			}

			public override bool IsValid( object value )
			{
				// If this is bound to something that's not a string, that's a programmer error, not a validation error
				var stringValue = (string)value;

				if ( value == null )
					return false;

				return SpAgent.Product.Activation.IsWellFormedKey( stringValue );
			}
		}
	}
}