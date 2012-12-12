/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */
namespace Sp.Samples.IntegratingObfuscators
{
	using Slps.ProtectionAttributes;
	using Sp.Samples.IntegratingObfuscators.Library;
	using System;

	class Program
    {
        // TODO: Choose an Obfuscator by including the appropriate Sp.Obfuscation.*.targets in the csproj files.
        // Some Obfuscators need to be installed on your machine to work with the provided .targets files.
        
        // Currently provided samples for Obfuscator Integration:
        // - Babel (http://babelfor.net/)

        // Notes:
        // Each assembly (the exe and the library) is Obfuscated and then Protected as individual post compilation phases after they are compiled. 
        // The bin folder will always contain the obfuscated/protected assemblies.
        // Some Obfuscators support features (e.g. suppressing ILDasm on the output assemblies) that prevent Code Protection - these must be disabled
        
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