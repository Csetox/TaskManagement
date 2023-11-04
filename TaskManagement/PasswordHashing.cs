using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement
{/// <summary>
/// This class is used for hashing the password. It uses PBKDF2, and the static RandomNumberGenerator class for generating the salt. 
/// </summary>
    public static class PasswordHashing
    {
        private const int saltSize = 16;
        private const int keySzie = 32;
        private const int iterations = 50000;

        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

        private const char segmentDelimiter = ':';

        public static string Hash(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, hashAlgorithm, keySzie);

            return string.Join(segmentDelimiter, Convert.ToHexString(hash), Convert.ToHexString(salt), iterations, hashAlgorithm);

        }

        public static bool Verify(string input, string hashedString)
        {
            string[] segments = hashedString.Split(segmentDelimiter);
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName hashAlgorithm = new HashAlgorithmName(segments[3]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, hashAlgorithm, hash.Length);

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);



        }

    }
}
