using ManyConsole.CommandLineUtils;
using Sp.Agent;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                SpAgent.Configuration.VerifyStoresInitialized();

				var commands = GetCommands();
				return ConsoleCommandDispatcher.DispatchCommand( commands, args, Console.Out );
			}
            catch ( Exception ex )
            {
                Console.Error.WriteLine( "Error: " + ex );
				return 1;
            }
        }

		public static IEnumerable<ConsoleCommand> GetCommands()
		{
			return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs( typeof( Program ) );
		}
	}
}
