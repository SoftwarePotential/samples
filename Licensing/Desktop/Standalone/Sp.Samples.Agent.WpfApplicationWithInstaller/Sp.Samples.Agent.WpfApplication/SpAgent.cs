/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */
namespace Sp.Samples.Agent.WpfApplication
{
	using System;
	using System.IO;
	using Sp.Agent;
	using Sp.Agent.Configuration;

	static partial class SpAgent
	{
		static IProductContext _product;
		static IAgentContext _agent;

		// Should be called only once during application startup
		public static void Initialize()
		{
			// NOTE: this is the 5 digit short code associated with your specific permutation .DLLs 
			// (See partial class sibling file: SpAgent.Generated.cs)
			_agent = AgentContext.For( PermutationShortId );	
			_agent.Configure( x => x
				// NOTE: Sp.Agent expects this folder to be initialized, i.e. your .msi installer needs to create and permission this folder
				.WithExternallyInitializedStore( SharedDirectoryInitializedByInstaller().FullName )				
				.CompleteWithDefaults() );

			_product = _agent.ProductContextFor( ProductName,ProductVersion ); 

			//Verify that the license store has been initialized
			Product.Stores.Initialization().Verify();
		}

		static DirectoryInfo SharedDirectoryInitializedByInstaller()
		{
			// NOTE the location computed here needs to be guaranteed by your installer to:-
			// a) always exist, i.e., the installer should unconditionally create the folder
			// b) be writable by all users whose identity will be used to execute protected code, i.e., it should give Read and Write permissions to an appropriate Security Group
			return new DirectoryInfo( Path.Combine(
				Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData ),
				ProductDirectoryPrefix + " " + ProductName,
				"Licenses" ) );
		}

		// Provides access to Product-level information from the Software Potential Agent.
		// Should not be accessed without first calling SpAgent.Initialize 
		public static IProductContext Product
		{
			get { return _product; }
		}

		// Triggers an Online Activation from the Software Potential service of the license with the specified activationKey identifier.
		// Should not be accessed without first calling SpAgent.Initialize.
		public static void ActivateLicense( string activationKey )
		{
			Product.Activation.OnlineActivate( activationKey );
		}

		// Removes a license from the license store
		public static void RemoveLicense( string activationKey )
		{
			Product.Stores.Delete( activationKey );
		}

		// Validates activation key format
		public static bool IsActivationKeyWellFormed( string activationKey )
		{
			return Product.Activation.IsWellFormedKey( activationKey );
		}
	}
}