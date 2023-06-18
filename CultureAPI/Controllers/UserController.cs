using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.DataLayerAbstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace CultureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {

        private readonly IUsersRepository _usersRepository;


        public UserController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpPatch("update-user/{id:int}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int id, UserUpdateDTO user)
        {
            int result = await _usersRepository.UpdateUser(id, user);
            if(result == 0)
            {
                return NotFound();
            }

            return Ok("Дані користувача успішно оновлені");
        }

        [HttpGet("get-user/{id:int}")]
        public async Task<ActionResult> GetUserInformation ([FromRoute] int id )
        {
            var result = await _usersRepository.GetUserById(id);
            if(result == null)
            {
                return NotFound("Користувач відстутній в мережі");
            }
            return Ok(result);
        }
    }
}
