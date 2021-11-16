using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Helper
{
    public static class Util
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };
        public static bool IsEmail(string input)
        {
            var regEx = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            return regEx.IsMatch(input);
        }

        public static string GenerateServerReference()
        {
            return DateTime.Now.Ticks.ToString();
        }


        public static string MaskPhoneNumber(this string phonenumber)
        {
            if (phonenumber.Length > 8)
            {
                var lastDigits = phonenumber.Substring(phonenumber.Length - 4, 4);
                var maskedPhonenumber = string.Concat(new String('*', phonenumber.Length - lastDigits.Length), lastDigits);
                phonenumber = maskedPhonenumber;
            }

            return phonenumber;
        }
        public static string CleanUpNumber(this string input)
        {
            input = new string(input.Where(char.IsDigit).ToArray());
            if (input.StartsWith("0234"))
                input.TrimStart('0');
            else if (input.StartsWith("0"))
            {
                input = "234" + input.Substring(1, input.Length - 1);
            }
            return input;
        }

        public static string MaskBvn(this string bvn)
        {
            if (bvn.Length > 8)
            {
                var lastDigits = bvn.Substring(bvn.Length - 4, 4);
                var maskedBvn = string.Concat(new String('*', bvn.Length - lastDigits.Length), lastDigits);
                bvn = maskedBvn;
            }

            return bvn;
        }

        public static string ToMaskedAccountNumber(this string accountNumber)
        {
            if (accountNumber.Length > 3)
            {
                var lastDigits = accountNumber.Substring(accountNumber.Length - 4, 4);
                var maskedAccountNumber = string.Concat(new String('X', accountNumber.Length - lastDigits.Length), lastDigits);
                accountNumber = maskedAccountNumber;
            }
            return accountNumber;
        }

        public static string MaskPan(string pan)
        {
            var firstDigits = pan.Substring(0, 6);
            var lastDigits = pan.Substring(pan.Length - 4, 4);
            var requiredMask = new string('X', pan.Length - firstDigits.Length - lastDigits.Length);
            var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
            return Regex.Replace(maskedString, ".{4}", "$0 ").Trim();
        }

        public static string SerializeAsJson<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public static T DeserializeFromJson<T>(string input)
        {

            return JsonConvert.DeserializeObject<T>(input, settings);
        }

     
        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            var lowerString = s.ToLower();
            // Return char and concat substring.
            return char.ToUpper(lowerString[0]) + lowerString.Substring(1);
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string RemoveCertainStrings(string input)
        {
            List<char> ListOfStringsToRemove = new List<char>() { '+', '(', ')' };
            foreach (var c in ListOfStringsToRemove)
            {
                input = input.Replace(c.ToString(), string.Empty);
            }
            return input;
        }

        public static string Encryptor(string request, string secretKey)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            byte[] vectorKeyBytes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            var encryptor = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 256,
                BlockSize = 128,
                IV = vectorKeyBytes,
                Key = secretKeyBytes
            };

            var plainBytes = Encoding.UTF8.GetBytes(request);
            var EncryptedInByte = encryptor.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(EncryptedInByte);
        }

        public static string Decryptor(string encryptedData, string secretKey)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            byte[] vectorKeyBytes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var decryptor = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 256,
                BlockSize = 128,
                IV = vectorKeyBytes,
                Key = secretKeyBytes,
            };

            var encryptedTextByte = Convert.FromBase64String(encryptedData);
            var DecryptedInBytes = decryptor.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
            return Encoding.UTF8.GetString(DecryptedInBytes);
        }

        public static T DecryptRequest<T>(string requestData, string secretKey)
        {
            var decryptedRequest = Decryptor(requestData, secretKey);
            var deserializedRequest = DeserializeFromJson<T>(decryptedRequest);

            return deserializedRequest;
        }

        public static bool IsBase64String(string input)
        {
            Span<byte> buffer = new Span<byte>(new byte[input.Length]);
            return Convert.TryFromBase64String(input, buffer, out int bytesParsed);
        }

        public static T Sanitize<T>(this T request, string[] propertiesToMask)
        {
            var type = request.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.Name.ToLower() == "string")
                {
                    if (propertiesToMask.Contains(property.Name))
                    {
                        if (property.Name.ToLower() == "pan" || property.Name.ToLower() == "cardpan")
                        {
                            property.SetValue(request, MaskPan(property.GetValue(request).ToString()));
                        }
                        else
                        {
                            property.SetValue(request, "*******");
                        }
                    }
                }
                else if (propertiesToMask.Contains(property.Name))
                {
                    property.SetValue(request, null);
                }
            }

            return request;
        }

        public static T Clone<T>(this T request)
        {
            var serializedRequest = SerializeAsJson(request);
            return DeserializeFromJson<T>(serializedRequest);
        }

        public static string GetEnumDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                       .GetMember(enumValue.ToString())
                       .First()
                       .GetCustomAttribute<DescriptionAttribute>()?
                       .Description ?? enumValue.ToString();
        }

        public static bool ContainsStringValue<T>(this T dataObject, string input)
        {
            var type = dataObject.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.Name.ToLower() == "string")
                {
                    var propValue = (string)property.GetValue(dataObject);
                    if (propValue.ToLower() == input.ToLower())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
