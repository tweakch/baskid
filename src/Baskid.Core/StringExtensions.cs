// The code in this solution is awesome!

using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Baskid.Core
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions {WriteIndented = true});
        }
    }

    public static class StringExtensions
    {
        public static string ToHash(this string input, string hashName = "MD5")
        {
            using (var algorithm = HashAlgorithm.Create(hashName)) //or SHA256, SHA512 etc.
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}