using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.DataLayerAbstractions;
using CultureAPI.Domain.Models;
using CultureAPI.Domain.ServiceAbstractions;
using CultureAPI.Infrastructure.DataLayer.DataLayerModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CultureAPI.Infrastructure.Services
{
    public class JwtGenerationService: IJwtGenerationService
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _usersRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        //private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtGenerationService(IConfiguration configuration, IAuthService authService, 
            IRefreshTokensRepository refreshTokensRepository, IUsersRepository usersRepository)
        {
            _configuration = configuration;
            //_tokenValidationParameters = validationParameters;
            _authService = authService;
            _refreshTokensRepository = refreshTokensRepository;
            _usersRepository = usersRepository;
        }
        public async Task<AuthResult> GenerateJwtToken(string email, int id)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretTokenKey = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                  new[]
                  {
                      new Claim("Id", id.ToString()),
                      new Claim(JwtRegisteredClaimNames.Sub, email),
                      new Claim(JwtRegisteredClaimNames.Email, email),
                      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                      new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                  }),

                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretTokenKey), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            string serializedToken = jwtTokenHandler.WriteToken(token);
            
            RefreshToken refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                Token = _authService.GenerateRandomString(20),
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                isRevoked = false,
                isUsed = false,
                UserId = id
            };

            await _refreshTokensRepository.AddRefreshToken(refreshToken);

            return new AuthResult()
            {
                Token = serializedToken,
                RefreshToken = refreshToken.Token,
                Result = true,
            };
        }

        public async Task<AuthResult> ValidateAndGenerateToken(TokenRequestDTO tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretTokenKey = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretTokenKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = false,
            };
            try
            {
                var tokenInVerification = jwtTokenHandler.
                    ValidateToken(tokenRequest.Token, tokenValidationParameters, out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return null;
                    }
                }

                var utcExpiryDate = long.Parse(tokenInVerification.Claims.
                    FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                if (expiryDate > DateTime.Now)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Tokens are expired"
                        }
                    };
                }

                RefreshToken storedToken = await _refreshTokensRepository.GetToken(tokenRequest);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid tokens, token = null",
                        }
                    };
                }

                if (storedToken.isUsed)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid tokens",
                        }
                    };
                }

                if (storedToken.isRevoked)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid tokens",
                        }
                    };
                }

                var jti = tokenInVerification.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid tokens",
                        }
                    };
                }

                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Tokens are expired",
                        }
                    };
                }

                TokenUserModel tokenUser = await _usersRepository.GetUserById(storedToken.UserId);
                await _refreshTokensRepository.UpdateTokenStatus(storedToken.Token, true);

                return  await GenerateJwtToken(tokenUser.Email, tokenUser.Id);
            }
            catch (Exception e)
            {
            
                return new AuthResult()
                {
                    Result = false,
                   Errors = new List<string>()
                   {
                           "ServerError"
                   }
                };
            }
        }

        DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeValue.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTimeValue;
        }

    }
}
