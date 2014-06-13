// NB This file is auto-generated via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
// 
// TODO: IF YOU MODIFY THIS FILE, CONSIDER MOVING ANY MODIFIED METHODS (AND/OR 
// RENAMING THIS FILE) SO NUGET PACKAGE UPDATES CANNOT RESULT IN YOU INADVERTENTLY 
// UNDOING CHANGES YOU HAVE MADE

using Sp.Agent;
using Sp.Agent.Configuration.Product.Activation;
using Sp.Agent.Configuration.Product.Activation.Internal;
using System;
using System.Diagnostics;

namespace Sp.Agent
{
	/// <summary>
	/// This portion of the partial class allows one to wire in a Product Customization without referring directly to the Product Identity or Permutation Ids.
	/// </summary>
	/// <remarks>
	/// The default implementation is intended to give an example of how the configuration chain looks like.
	/// As-is, it has a null effect - i.e. it just requests default behaviors for almost everything.
	/// ([Assuming you haven't added any customizations,] This file and the code within it can actually be 
	/// deleted as omitting any configuration just defaults all behaviors)
	/// </remarks>
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
					.WithBaseUriPolicy( _ => new Uri( "http://localhost/LicenseIssueWeb/" ) )
						//.WithEndpointSelectionPolicyDefault()
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

		/// <summary>
		/// <para>Provides standardized processing of Licensing-related command-line parameters.</para>
		/// <para>To be invoked from your application's entry point.</para>
		/// </summary>
		/// <remarks>
		/// This portion of the partial class allows one to customize whether/how 
		/// messages from the command line processing logic will be emitted to a Console.
		/// It is safe to delete this class, though the absence of messages may make 
		/// error diagnosis more difficult.
		/// </remarks>
		static partial class CommandLineProcessing
		{
			static partial void ReportInstallationAction( string action )
			{
				Console.WriteLine( action );
			}
		}
	}
}