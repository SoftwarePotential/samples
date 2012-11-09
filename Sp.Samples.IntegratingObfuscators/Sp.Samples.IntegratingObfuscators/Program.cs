using Sp.Samples.IntegratingObfuscators.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sp.Samples.IntegratingObfuscators
{
    class Program
    {
        static void Main( string[] args )
        {
            MainObfuscatable();
        }

        private static void MainObfuscatable()
        {
            var service = new AnswerService();
            Console.WriteLine( service.GetAnswer() );
        }
    }
}
