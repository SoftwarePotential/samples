/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */
namespace Sp.Samples.Agent.WpfApplication.BusinessLogic
{
	using Slps.ProtectionAttributes;

	static class MyAlgorithms
	{
		[Feature( "FeatureA" )]
		public static void AccessFeatureA()
		{

		}

		[Feature( "FeatureB" )]
		public static void AccessFeatureB()
		{

		}
	}
}
