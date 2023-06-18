using System.Security.Cryptography;
using System.Text;
using System;
using CultureAPI.Domain.Models;
using CultureAPI.Controllers.DTO;

namespace CultureAPI.Domain.ServiceAbstractions
{
    public interface IAuthService
    {
        public string CreatePasswordHash(string userPassword, string userSalt);

        public bool CheckPasswordHash(string incomingPassword, string userHash, string userSalt);

        public string GenerateRandomString(int length);

    }
}
