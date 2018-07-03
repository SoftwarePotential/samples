using Sp.Agent;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApp.Licensing
{
	public class SpAgentActivation
	{
		public static void ActivateOnline( string activationKey, Action<Task,string> continueWith )
		{
			var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
			SpAgent.Product.Activation.OnlineActivateAsync( activationKey )
				.ContinueWith( task => continueWith( task, activationKey ), CancellationToken.None, TaskContinuationOptions.None, uiContext );
		}
	}
}