using App.Commands;
using Application.Common.Exceptions;
using Application.Common.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class AppUtility
    {
        public static void BrokerFailureMessage(string message)
        {
            throw new CustomException(message);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string DecryptString(string encyptedString)
        {
            byte[] b;
            string decryptedString;
            try
            {
                b = Convert.FromBase64String(encyptedString);
                decryptedString = Encoding.ASCII.GetString(b);
                return decryptedString;
            }
            catch (FormatException fe)
            {
                throw fe;
            }
        }

        public static bool IsEqualString(string encrytedString, string plainString)
        {
            byte[] b;
            string decryptedString;
            try
            {
                b = Convert.FromBase64String(encrytedString);
                decryptedString = Encoding.ASCII.GetString(b);
                //check if strings are equal
                var isValid = decryptedString.ToLower().Equals(plainString.ToLower());
                return isValid;
            }
            catch (FormatException fe)
            {
                throw fe;
            }
        }

        public static string EnryptString(string plainString)
        {
            byte[] b = Encoding.ASCII.GetBytes(plainString);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        public static bool AllCharactersSame(string word)
        {
            int n = word.Length;
            for (int i = 1; i < n; i++)
                if (word[i] != word[0])
                    return false;
            return true;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static byte[] GetObjectByte<T>(T value)
        {
            var stringMessage = JsonConvert.SerializeObject(value, Formatting.None);
            return Encoding.UTF8.GetBytes(stringMessage);
        }
    
        public static T GetObjectFromByte<T>(byte[] value)
        {
            var bytesAsString = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject<T>(bytesAsString);
        }

        public static void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var cipher = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = cipher.Key;
                passwordHash = cipher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public static bool VerifyInformation(string Password, byte[] storedSalt, byte[] storedHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                return computed.SequenceEqual(storedHash);
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmaic = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmaic.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }

                return true;
            }

        }

    }
}
