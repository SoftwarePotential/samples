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
