using System;
using System.Collections.Generic;
using System.Linq;

namespace Sp.Samples.IntegratingObfuscators.Library
{
    public class AnswerService
    {
        public string GetAnswer()
        {
            return GetAnswerObfuscatable();
        }

        private static string GetAnswerObfuscatable()
        {
            return "42";
        }
    }
}
