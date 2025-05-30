using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace QR_Checking_App
{
    class SHA256Converter
    {
        public static string ConvertToSHA256(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = sha.ComputeHash(inputBytes);
            string hexString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return hexString;
        }
    }
}
