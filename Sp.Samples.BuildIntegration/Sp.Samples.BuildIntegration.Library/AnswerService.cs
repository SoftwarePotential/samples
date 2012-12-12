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

	public class AnswerService
    {
        public string GetAnswer()
        {
            MethodStatus.PrintOf( () => GetAnswerObfuscatable(), "GetAnswerObfuscatable" );
            return GetAnswerObfuscatable();
        }

        [Feature]
        private static string GetAnswerObfuscatable()
        {
            return "42";
        }
    }
}
