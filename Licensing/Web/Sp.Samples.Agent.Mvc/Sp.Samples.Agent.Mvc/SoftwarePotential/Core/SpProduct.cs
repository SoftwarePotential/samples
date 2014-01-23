// NB This file is auto-generated via the Sp.Product-Demo_10 NuGet package.
//
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

namespace Sp.Agent
{
	static class SpProduct
	{
		public const string Vendor = @"InishTech";
		public const string Name = @"Demo";
		public const string Version = @"1.0";
	}
}

namespace Sp.Samples.Agent.Mvc
{
	/// <summary>Defines features and properties associated with the Software Potential Product Definition for 'Demo' - '1.0'</summary>
	static partial class @Demo_10
	{
		/// <summary>Defines Attributes used to mark code for Protection by the Software Potential Code Protector, together with the (string) Names of the features for the purposes of generating custom licensing queries.</summary>
		public static partial class Features
		{
			#region Feature2
			/// <summary>Require a License for 'Demo' - '1.0' with the 'Name: Feature2' Feature to be present.</summary>
			[System.AttributeUsage( System.AttributeTargets.Constructor | System.AttributeTargets.Method )]
			public sealed class @Feature2 : LicensedFeatureAttribute
			{
				public const string Name = @"Feature2";
 
				public @Feature2() : base( Name ) {}
			}
			#endregion

			#region Feature1
			/// <summary>Require a License for 'Demo' - '1.0' with the 'Name: Feature1' Feature to be present.</summary>
			[System.AttributeUsage( System.AttributeTargets.Constructor | System.AttributeTargets.Method )]
			public sealed class @Feature1 : LicensedFeatureAttribute
			{
				public const string Name = @"Feature1";
 
				public @Feature1() : base( Name ) {}
			}
			#endregion

			#region Feature3
			/// <summary>Require a License for 'Demo' - '1.0' with the 'Name: Feature3' Feature to be present.</summary>
			[System.AttributeUsage( System.AttributeTargets.Constructor | System.AttributeTargets.Method )]
			public sealed class @Feature3 : LicensedFeatureAttribute
			{
				public const string Name = @"Feature3";
 
				public @Feature3() : base( Name ) {}
			}
			#endregion

			#region LicensedFeatureAttribute
			/// <summary>
			/// <para>Marks a method to be replaced with Protected code by the Software Potential Code Protector during the build process.</para>
			/// <para>Typically one should use the specific Feature attributes in this <c>Features</c> class in preference to using this directly.</para>
			/// <para>Updating the NuGet package associated with this file will pick up new Features.</para>
			/// </summary>
			/// <remarks>This class fulfills two key requirements in order to be identified by the Software Potential Code Protector versions >= 3.2.1942 as a Protection Attribute just as Slps.ProtectionAttributes.FeatureAttribute is:
			/// - class name is LicensedFeatureAttribute (even after Obfuscation)
			/// - bears a read/write property called FeatureName which has a value matching that in the Software Potential Product Definition (even after Obfuscation)
			/// </remarks>
			[System.AttributeUsage( System.AttributeTargets.Constructor | System.AttributeTargets.Method )]
			[System.Reflection.Obfuscation]
			public abstract class LicensedFeatureAttribute : System.Attribute
			{
				protected LicensedFeatureAttribute( string featureName )
				{
					FeatureName = featureName;
				}

				[System.Reflection.Obfuscation]
				public string FeatureName { get; set; }
			}
			#endregion
		}

		#region LicenseAttribute
		/// <summary>Require any License for 'Demo' - '1.0' to be present.</summary>
		[System.AttributeUsage( System.AttributeTargets.Constructor | System.AttributeTargets.Method )]
		public sealed class LicenseAttribute : Features.LicensedFeatureAttribute
		{
			public LicenseAttribute() : base( string.Empty ) { }
		}
		#endregion
	}
}