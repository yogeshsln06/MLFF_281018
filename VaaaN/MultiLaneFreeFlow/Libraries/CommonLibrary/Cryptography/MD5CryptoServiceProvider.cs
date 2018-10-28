using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;


namespace VaaaN.MLFF.Libraries.CommonLibrary.Cryptography
{
	/// <summary>
	/// Provides various MD5 functions
	/// </summary>
    public class MD5CryptoServiceProvider
    {
        /// <summary>
        /// Computes the MD5 hash for a unicode string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeMd5(string str)
        {
            byte[] plainText = System.Text.Encoding.Unicode.GetBytes(str);

            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] hash = md5Hasher.ComputeHash(plainText);

            string strHex = "";
            foreach (byte b in hash)
            {
                strHex += string.Format("{0:x2}", b);
            }

            return strHex;
        }

        public static int ComputeChecksum(string value)
        {
            //LUHN Algorithm
            return value
            .Where(c => Char.IsDigit(c))
            .Reverse()
            .SelectMany((c, i) => ((c - '0') << (i & 1)).ToString())
            .Sum(c => c - '0') % 10;
        }
    }
}
