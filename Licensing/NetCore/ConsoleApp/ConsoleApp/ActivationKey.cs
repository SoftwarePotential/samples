using Sp.Agent;

namespace ConsoleApp
{
	static class ActivationKey
	{
		static readonly int _requiredLength = 29;

		public static bool IsWellFormed( string key ) => key.Length == _requiredLength && SpAgent.Product.Activation.IsWellFormedKey( key );
	}
}
