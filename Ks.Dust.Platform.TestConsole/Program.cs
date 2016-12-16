using System;
using Dust.Platform.Service;
using SHWDTech.Platform.Utility;

namespace Ks.Dust.Platform.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var secret = Globals.NewIdentityCode();
            Console.WriteLine(secret);
            var secretHase = Helper.GetHash(secret);

            Console.WriteLine(secretHase);
            Console.ReadKey();
        }
    }
}
