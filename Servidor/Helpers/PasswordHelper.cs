using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Servidor.Helpers
{
    public class PasswordHelper
    {
        public static (string pass, string salt) GenerateNewEncodedPassword(string pass)
        {
            var salt = GenerateSalty(pass.Length);
            return (EncodedPassword(pass, salt), salt);
        }

        private static string GenerateSalty(int length)
        {
            const string charsAllowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var chars = Enumerable.Range(0, length).Select(n => charsAllowed[random.Next(charsAllowed.Length)]).ToArray();

            return new string(chars);
        }

        public static string EncodedPassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);

            return EncodedPasswordMd5(pass);
        }

        private static string EncodedPasswordMd5(string pass)
        {
            MD5 md5;

            md5 = new MD5CryptoServiceProvider();
            byte[] originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }
    }
}