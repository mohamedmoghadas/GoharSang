using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace System
{

    public static class CreatHash
    {
        

        public static string Hash(string str)
        {
            HashAlgorithm alg = new MD5CryptoServiceProvider();
            var buffer = alg.ComputeHash(UTF8Encoding.UTF8.GetBytes(str));
            return Convert.ToBase64String(buffer);
        }


        public static string Encrypt(string input)
        {
            string Keyvalue = "sblw-3hn8-sqoy19";

            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(Keyvalue);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string input)
        {
            string Keyvalue = "sblw-3hn8-sqoy19";

            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(Keyvalue);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string HashPass(string str)
        {
          string pass1=  CreatHash.Hash(str);
            string pass2 = CreatHash.Encrypt(pass1);
            return pass2;

        }

    }
}