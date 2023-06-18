using System;
using System.Text;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using Microsoft.Extensions.Primitives;
using CultureAPI.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using CultureAPI.Controllers.DTO;
using CultureAPI.Infrastructure.DataLayer.DataLayerModels;
using CultureAPI.Domain.ServiceAbstractions;

namespace CultureAPI.Infrastructure.Services
{
    public class AuthService: IAuthService
    {
        private static Random random = new Random();

        public string CreatePasswordHash(string userPassword, string userSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                hmac.Key = Encoding.UTF8.GetBytes(userSalt);
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userPassword));
                return Convert.ToBase64String(hash);
            }
        }
        public bool CheckPasswordHash(string incomingPassword, string userHash, string userSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                hmac.Key = Encoding.UTF8.GetBytes(userSalt);
                byte[] hashForCheck = hmac.ComputeHash(Encoding.UTF8.GetBytes(incomingPassword));
                return userHash.SequenceEqual(Convert.ToBase64String(hashForCheck));
            }
        }

        DateTime UnixTimeStampToDateTime (long unixTimeStamp)
        {
            var dateTimeValue = new DateTime(1970, 1, 1, 0, 0 ,0, 0, DateTimeKind.Utc);
            dateTimeValue.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTimeValue;
        }
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
