using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Coaching.Helper
{
    public static class SecurityHelper
    {
        private const string AES_IV = "TXgvwtjtBdk6N3QD";
        private const string AES_KEY = "HdKMxBKUfJVqmjKuYXC3H5PUSuGctbZJ";

        public static string EncryptText(string plainText)
        {

            var iv = Encoding.UTF8.GetBytes(AES_IV);
            var key = Encoding.UTF8.GetBytes(AES_KEY);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            var rijndael = Rijndael.Create();
            var encryptor = rijndael.CreateEncryptor(key, iv);
            var memoryStream = new MemoryStream(plainBytes.Length);

            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();

            var cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            var a = Convert.ToBase64String(cipherBytes);
            return Convert.ToBase64String(cipherBytes);
        }

        public static string DecryptText(string cipherText)
        {
            var iv = Encoding.UTF8.GetBytes(AES_IV);
            var key = Encoding.UTF8.GetBytes(AES_KEY);

            var cipherBytes = Convert.FromBase64String(cipherText);
            var plainBytes = new byte[cipherBytes.Length];

            var rijndael = Rijndael.Create();
            var decryptor = rijndael.CreateDecryptor(key, iv);

            var memoryStream = new MemoryStream(cipherBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            var decryptedByteCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            var plainText = Encoding.UTF8.GetString(plainBytes, 0, decryptedByteCount);
            return plainText;
        }
    }
}
