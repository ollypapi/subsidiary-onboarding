using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Application.Implementation
{
    public class CryptoResource : ICryptoResource
    {
        private readonly ILogger<CryptoResource> logger;
        private readonly IConfiguration configuration;
        private string CryptoKey { get; set; }

        public CryptoResource(ILogger<CryptoResource> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            CryptoKey = configuration["ResourceSecure:EncKey"];
        }
        public string Decrypt(string cypherText)
        {
            var des = CreateDES(CryptoKey);
            var ct = des.CreateDecryptor();
            var input = Convert.FromBase64String(cypherText);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(output);
        }

        public string Encrypt(string text)
        {
            var des = CreateDES(CryptoKey);
            var ct = des.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(text);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(output);
        }

        public string MaskCardPan(string cardPan)
        {
            var firstSixDigits = cardPan.Substring(0, 6);
            var lastFourDigits = cardPan.Substring(cardPan.Length - 4, 4);
            var requiredMask = new string('*', cardPan.Length - firstSixDigits.Length - lastFourDigits.Length);
            var response = string.Concat(firstSixDigits, requiredMask, lastFourDigits);

            logger.LogInformation($"MASKED_PAN: {response}");
            return response;
        }

        private static TripleDES CreateDES(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            return des;
        }
    }
}
