using System;
using System.Security.Cryptography;
using System.Text;

namespace Sho.BankIntegration.Privatbank.Utils
{
    public static class CryptographyExtensions
    {
        public enum HashType
        {
            Md5, Sha1
        }

        /// <summary>
        /// Calculates the MD5 hash for the given string.
        /// </summary>
        /// <returns>A 32 char long hash.</returns>
        public static string GetHashMd5(this string input)
        {
            return ComputeHash(HashType.Md5, input);
        }

        /// <summary>
        /// Calculates the SHA-1 hash for the given string.
        /// </summary>
        /// <returns>A 40 char long hash.</returns>
        public static string GetHashSha1(this string input)
        {
            return ComputeHash(HashType.Sha1, input);
        }

        public static string ComputeHash(HashType hashType, string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            HashAlgorithm hasher = GetHasher(hashType);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = hasher.ComputeHash(inputBytes);
            StringBuilder hash = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                hash.Append(string.Format("{0:x2}", b));
            }

            return hash.ToString();
        }

        private static HashAlgorithm GetHasher(HashType hashType)
        {
            switch (hashType)
            {
                case HashType.Md5:
                {
                    return new MD5CryptoServiceProvider();
                }
                case HashType.Sha1:
                { 
                    return new SHA1Managed();
                }
                default:
                { 
                    throw new ArgumentOutOfRangeException("Specified hash type is not supported");
                }
            }
        }
    }
}
