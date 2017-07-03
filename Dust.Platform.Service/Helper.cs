using System;
using System.Security.Cryptography;

namespace Dust.Platform.Service
{
    public class Helper
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
       
            var byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            var byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static int GetRate(double value)
        {
            if(value < 0.4) return 0;

            if (value >= 0.4 && value < 1.0) return 1;

            return 2;
        }
    }
}