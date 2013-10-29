// NB This file is auto-generated via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
// 
// CONSIDER RENAMING OR MOVING THIS FILE SO A PACKAGE UPDATE CANNOT UNDO ANY CHANGES YOU MAKE

using Sp.Agent;
using Sp.Agent.Configuration.Product.Activation;
using System.Diagnostics;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		/// <summary>
		/// 
		/// <para>TODO: YOUR CUSTOMIZATIONS OR DELETE THIS METHOD</para>
		/// 
		/// <para>Should be edited as appropriate if you wish to customize any aspects of how your Licensing policies should affect your application</para>
		/// </summary>
		/// <remarks>
		/// NB the name and namespace of the class needs to remain as-is in order for partial method to slot into the code in SpAgent.cs correctly
		/// </remarks>
		static partial void ConfigureProduct( IProductContext productContext )
		{
			//=====================================================================================
			// TODO: Tweak any settings as desired 
			// (OR YOU CAN DELETE THIS METHOD TO HAVE THE DEFAULT CONFIGURATION BE APPLIED INSTEAD)
			//=====================================================================================

			productContext.Configure( configure => configure
				.Activation.Customize( activation => activation
					.WithTagging( AddActivationTags )
					.WithTransmission( activationTransmission => activationTransmission
						.WithRetryPolicyDefault()
						.WithEndpointSelectionPolicyDefault()
						.BeforeEachAttempt( WhenActivating )
						.CompleteWithDefaults() )
					.CompleteWithDefaults() )
				.CompleteWithDefaults() );
		}

		static void AddActivationTags( IActivationTaggingContext context )
		{
			Debug.WriteLine( "State passed to Activate() method: " + context.State );
			// e.g. context.AddTag("MYKEY"."MYVALUE");
		}

		static void WhenActivating( IActivationAttemptContext context )
		{
			Debug.WriteLine( "Activation attempt #" + (context.PreviousAttempts + 1) );
		}
	}
}