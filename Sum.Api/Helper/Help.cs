using System;
using System.Security.Cryptography;
using System.Text;

namespace Sum.Api.Helper
{
    public static class Help
    {
        public static string GeneratePassword(int length)
        {
            //const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            const string valid = "abcdefghijklmnopqrstuvwxyz1234567890";
            var res = new StringBuilder();
            var rnd = new Random();
            while (0 < length--) res.Append(valid[rnd.Next(valid.Length)]);
            return res.ToString();
        }

        public static string GenerateCode(int length)
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var res = new StringBuilder();
            var rnd = new Random();
            while (0 < length--) res.Append(valid[rnd.Next(valid.Length)]);
            return res.ToString();
        }

        public static string Hashing(string input)
        {
            byte[] hash;
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(input));
            }

            var sb = new StringBuilder();
            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
            return sb.ToString();

        }
    }
}