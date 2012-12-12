/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */
namespace Sp.Samples.IntegratingObfuscators.Library
{
    using Slps.ProtectionAttributes;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

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

        static MethodInfo MethodOf<T>( Expression<T> expression )
        {
            MethodCallExpression body = (MethodCallExpression)expression.Body;
            return body.Method;
        }

        static void PrintInfoOfMethod( MethodInfo method, string unobfuscatedName )
        {
            bool isObfuscated = method.Name != unobfuscatedName;
            Console.WriteLine( unobfuscatedName + " is obfuscated: " + isObfuscated );

            bool isProtected = method.GetCustomAttributes( typeof( FeatureAttribute ), true ).Length == 0;
            Console.WriteLine( unobfuscatedName + " is protected: " + isProtected );

            bool assemblyHasStrongName = method.DeclaringType.Assembly.GetName().GetPublicKeyToken().Length == 8;
            Console.WriteLine( "Assembly that declares " + unobfuscatedName + " has strong-name: " + assemblyHasStrongName );
        }
    }
}