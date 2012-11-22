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
