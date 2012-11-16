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
            MainObfuscatable();

            AssertObfuscated( MethodOf( () => MainObfuscatable() ) );
            
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

        public static MethodInfo MethodOf( Expression<Action> expression )
        {
            MethodCallExpression body = (MethodCallExpression)expression.Body;
            return body.Method;
        }

        private static void AssertObfuscated( MethodInfo method )
        {
            if (method.Name == "MainObfuscatable")
                Console.WriteLine( "MainObfuscatable() was not obfuscated" );
        }
    }
}
