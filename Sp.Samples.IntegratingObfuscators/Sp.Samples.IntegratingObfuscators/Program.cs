using Slps.ProtectionAttributes;
using Sp.Samples.IntegratingObfuscators.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sp.Samples.IntegratingObfuscators
{
    class Program
    {
        // TODO: Choose an Obfuscator by including the appropriate Sp.Obfuscation.*.targets in the csproj files.
        // Some Obfuscators need to be installed on your machine to work with the provided .targets files.
        
        // Currently provided samples for Obfuscator Integration:
        // - Babel (http://babelfor.net/)

        // Notes:
        // Each assembly (the exe and the library) are Protected and Obfuscated separately after they are compiled. 
        // The bin folder will always contain the obfuscated and protected assemblies.
        // Some Obfuscators support features (e.g. supressing ILdasm on the output assemblies) that prevent Code Protection - these must be disabled
        
        static void Main( string[] args )
        {
            MethodStatus.PrintOf( () => MainObfuscatable(), "MainObfuscatable" );
            MainObfuscatable();

            Console.ReadLine();
        }
         
        /// <summary>
        /// This method will be protected and an obfuscator should be able to obfuscate it
        /// </summary>
        [Feature]
        private static void MainObfuscatable()
        {
            var service = new AnswerService();
            Console.WriteLine( "The answer is: " + service.GetAnswer() );
        }
    }
}
