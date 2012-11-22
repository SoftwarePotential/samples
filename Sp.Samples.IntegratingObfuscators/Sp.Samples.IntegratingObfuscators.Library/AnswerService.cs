using Slps.ProtectionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sp.Samples.IntegratingObfuscators.Library
{
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
