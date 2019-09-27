using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Entities
{
    public static class Cryptography
    {
        #region Encrypt
        public static string EnryptString(string strEncrypted)
        {
            byte[] b = Encoding.UTF8.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        #endregion
        #region Decrypt
        public static string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = Encoding.UTF8.GetString(b);
            }
            catch
            {
                decrypted = "";
            }
            return decrypted;
        }
        #endregion
    }
}
