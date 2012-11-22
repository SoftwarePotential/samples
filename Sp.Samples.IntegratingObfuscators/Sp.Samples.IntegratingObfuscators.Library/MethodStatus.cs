using Slps.ProtectionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Sp.Samples.IntegratingObfuscators.Library
{
    public class MethodStatus
    {
        public static void PrintOf<T>( Expression<Func<T>> expression, string unobfuscatedName )
        {
            var method = MethodOf( expression );
            PrintInfoOfMethod( method, unobfuscatedName );
        }

        public static void PrintOf( Expression<Action> expression, string unobfuscatedName )
        {
            var method = MethodOf( expression );
            PrintInfoOfMethod( method, unobfuscatedName );
        }

        private static MethodInfo MethodOf<T>( Expression<T> expression )
        {
            MethodCallExpression body = (MethodCallExpression)expression.Body;
            return body.Method;
        }

        private static void PrintInfoOfMethod( MethodInfo method, string unobfuscatedName )
        {
            bool isObfuscated = method.Name != unobfuscatedName;
            Console.WriteLine( unobfuscatedName + " is obfuscated: " + isObfuscated );

            bool isProtected = method.GetCustomAttribute( typeof( FeatureAttribute ) ) == null;
            Console.WriteLine( unobfuscatedName + " is protected: " + isProtected );
        }
    }
}
