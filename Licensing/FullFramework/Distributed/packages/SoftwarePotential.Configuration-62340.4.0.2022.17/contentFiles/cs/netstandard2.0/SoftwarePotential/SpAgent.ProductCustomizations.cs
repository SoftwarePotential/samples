// NB This file is auto-generated via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
// 
// TODO: IF YOU MODIFY THIS FILE, CONSIDER MOVING ANY MODIFIED METHODS (AND/OR 
// RENAMING THIS FILE) SO NUGET PACKAGE UPDATES CANNOT RESULT IN YOU INADVERTENTLY 
// UNDOING CHANGES YOU HAVE MADE

using Sp.Agent.Configuration.Product.Activation;
using System;
using System.Diagnostics;
using System.Net;

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
						.WithEndpointSelectionPolicyDefault()
						.BeforeEachAttempt( WhenActivating )
						.WithProxyConfigurationPolicy( SetProxyConfigurationPolicy )
						.CompleteWithDefaults() )
					.WithDeviceLabelPolicy( SetDeviceLabelPolicy )
					.CompleteWithDefaults() )
				.CompleteWithDefaults() );
		}

		static void AddActivationTags( IActivationTaggingContext context )
		{
			Debug.WriteLine( "State passed to Activate() method: " + context.State );
			// e.g. context.AddTag("MYKEY","MYVALUE");
		}

		static void WhenActivating( IActivationAttemptContext context )
		{
			Debug.WriteLine( "Activation attempt #" + (context.PreviousAttempts + 1) );
		}

		/// <summary>
		/// <param name="activationEndpoint">
		/// The Activation Service endpoint used to submit Activation Requests.
		/// </param>
		/// <para>
		/// Replace the contents of this method to return a proxy to be used at activation.
		/// Return null if you do not wish to set a proxy.
		/// </para>
		/// </summary>
		/// <example>
		/// If targeting full framework, you can detect the address of the default proxy for the activation endpoint
		/// and return a proxy for this address with default credentials; you return NULL if no proxy assigned:
		/// <code>
		/// static IWebProxy SetProxyConfigurationPolicy( Uri activationEndpoint )
		/// {
		///		var proxiedAddress = WebRequest.DefaultWebProxy.GetProxy( activationEndpoint );
		///		if ( !activationEndpoint.Equals( proxiedAddress ) )
		///			return new WebProxy( proxiedAddress ) { UseDefaultCredentials = true };
		///		else
		///			return null;
		///	}
		/// </code>
		/// </example>
		static IWebProxy SetProxyConfigurationPolicy( Uri activationEndpoint )
		{
			return null;
		}

		/// <summary>
		/// <param name="context">
		/// <para>Context that allows you to set a custom Device Label</para>
		/// </param>
		/// Replace the contents of this method to customize the value of the DeviceLabel sent up on Activation.
		/// If this method is left unmodified the DeviceLabel will be set to the default value of Environment.MachineName
		/// </summary>
		static void SetDeviceLabelPolicy( IActivationDeviceLabelContext context )
		{
			// e.g. context.SetDeviceLabel("My custom label");
			Debug.WriteLine( "DeviceLabel passed to Activate() method: " + context.DeviceLabel );
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