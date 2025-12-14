using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgeClient.Classes
{
    public static class SimpleEncryptor
    {
        private static readonly string Key = "MySecretKey";

        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var result = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                result.Append((char)(text[i] ^ Key[i % Key.Length]));
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(result.ToString()));
        }

        public static string Decrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var bytes = Convert.FromBase64String(text);
            var encoded = Encoding.UTF8.GetString(bytes);

            var result = new StringBuilder();
            for (int i = 0; i < encoded.Length; i++)
            {
                result.Append((char)(encoded[i] ^ Key[i % Key.Length]));
            }
            return result.ToString();
        }
    }
}
