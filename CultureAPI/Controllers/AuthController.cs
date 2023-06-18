using CultureAPI.Controllers.DTO;
using Microsoft.AspNetCore.Mvc;
using CultureAPI.Domain.Models;
using CultureAPI.Infrastructure.DataLayer.DataLayerModels;
using CultureAPI.Domain.ServiceAbstractions;
using CultureAPI.Domain.DataLayerAbstractions;

namespace CultureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
       
        private readonly IJwtGenerationService _jwtGenerationService;

        private readonly IUsersRepository _usersRepository;
        public AuthController(IAuthService authService, IJwtGenerationService jwtGenerationService, IUsersRepository usersRepository) 
        { 
            _authService = authService;
            _jwtGenerationService = jwtGenerationService;
            _usersRepository = usersRepository;
        }

        [HttpPost("register-user")]
        public async Task<ActionResult<AuthResult>> ResisterUser([FromBody] UserRegistrationDTO userCredentials)
        {
           int userId = await _usersRepository.AddUser(userCredentials);
           User targetUser = await _usersRepository.GetUserByEmail(userCredentials.Email);
           return Ok(await _jwtGenerationService.GenerateJwtToken(targetUser.Email, targetUser.Id));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login([FromBody] UserLoginDTO userCredentials)
        {
            User targetUser = await _usersRepository.GetUserByEmail(userCredentials.Email);
            if (targetUser == null)
            {
                return NotFound(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    { 
                        "User not found"
                    }
                });
                
            }

            if (!_authService.CheckPasswordHash(userCredentials.Password, targetUser.Password, targetUser.PasswordSalt))
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Credentials check is failed"
                    }
                });
            }

            return Ok(await _jwtGenerationService.GenerateJwtToken(targetUser.Email, targetUser.Id));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] TokenRequestDTO tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var validationResult = await _jwtGenerationService.ValidateAndGenerateToken(tokenRequest);

                if (tokenRequest == null)
                {
                    return BadRequest(new AuthResult()
                    { 
                        Errors = new List<string>()
                        {
                            "Tokens are invalid"
                        }
                    });
                }

                return Ok(validationResult);
            }
            return BadRequest(new AuthResult() 
            { 
                Errors = new List<string>() 
                {
                    "Invalid characters"
                },
                Result = false,
            });

        }
    }
}
