using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extentions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Helper
{
    public class TokenizationHelper
    {
        IConfiguration _config;

        public TokenizationHelper(IConfiguration config)
        {
            _config = config;
        }

        public Token GetToken(TokenDataModel data)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:SecurityKey")));
            var claims = new Claim[] {

                new Claim(ClaimTypes.NameIdentifier, $"{data.CustomerId.ToString()}"),
                new Claim(ClaimTypes.Name, $"{data.FirstName} {data.LastName}"),
                new Claim(JwtRegisteredClaimNames.Email, $"{data.UserEmail}"),
                new Claim(ClaimTypes.MobilePhone, $"{data.MobileNumber}"),
                new Claim(ClaimTypes.Role, $"{data.JwtUserType.GetDescription()}"),
                new Claim("CIF", $"{data.CIF}"),
                new Claim("user-detail",  JsonConvert.SerializeObject(new UserData(){
                 CustomerName=$"{data.FirstName} {data.LastName}",
                 MobileNumber=data.MobileNumber,
                 UserEmail=data.UserEmail,
                 CustomerId=data.CustomerId,
                 UserType="Customer"

             }))

            };

            var expiresIn = DateTime.Now.AddMinutes(_config.GetValue<int>("Jwt:TimeoutMinutes"));
            var RefreshTokenExpiresIn = DateTime.Now.AddMinutes(_config.GetValue<int>("Jwt:TimeoutMinutes")+3);
            var token = new JwtSecurityToken(
                issuer: _config.GetValue<string>("Jwt:Issuer"),
                audience: _config.GetValue<string>("Jwt:Audience"),
                claims: claims,
                notBefore: DateTime.Now,
                expires: expiresIn,
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            string refreshToken = GetRefreshToke();
            var tokenData = new Token { Access_Token = jwtToken, ExpiresIn = expiresIn, RefreshToken= refreshToken, RefreshToken_ExpiresIn = RefreshTokenExpiresIn };
            return tokenData;
        }

        public string GetRefreshToke()
        {
            var randomNumber = new Byte[12];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
