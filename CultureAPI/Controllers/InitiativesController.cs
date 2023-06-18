using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.DataLayerAbstractions;
using CultureAPI.Domain.Models;
using CultureAPI.Domain.ServiceAbstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace CultureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InitiativesController : ControllerBase
    {
        private readonly IInitiativesReporitory _initiativesReporitory;
        private readonly IUsersRepository _usersRepository;
        private readonly IMailService _mailService;

        public InitiativesController(IInitiativesReporitory initiativesReporitory, IUsersRepository usersRepository, IMailService mailService)
        {
            _initiativesReporitory = initiativesReporitory;
            _usersRepository = usersRepository;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Initiative>>> GetAllInitiatives()
        {
            IEnumerable<Initiative> initiatives = await _initiativesReporitory.GetAllInitiatives();
            return Ok(initiatives);
        }

        [HttpGet("top-initiatives")]
        public async Task<ActionResult<IEnumerable<Initiative>>> GetTopInitiatives()
        {
            IEnumerable<Initiative> initiatives = await _initiativesReporitory.GetTopInitiatives();
            return Ok(initiatives);
        }

        [HttpPost("create-initiative")]
        public async Task<ActionResult<int>> CreateInitiative([FromBody] InitiativeDTO initiative)
        {
            int id = await _initiativesReporitory.CreateInitiative(initiative);
            return Ok(id);
        }

        [HttpPatch("support-initiative/{id:int}")]
        public async Task<ActionResult> SupportInitiative([FromRoute] int id, [FromBody] SupportInitiativeDTO supportInitiative)
        {
            int initiativeCheckCode = await _initiativesReporitory.CheckSupportedInitiative(id, supportInitiative.UserId);
            if (initiativeCheckCode == 1) {
                return BadRequest("Ініціатива вже підтримана користувачем");
            }
            await _initiativesReporitory.SupportInitiative(id, supportInitiative);
            var userToSend = await _usersRepository.GetUserById(supportInitiative.UserId);
            var initiative = await _initiativesReporitory.GetInitiativeForEmail(id);
            Email email = new Email { Attachments = null, Template = "Support", 
                ToEmail = userToSend.Email, EmailType = EmailType.Support };
            InitiativeRequest request = new InitiativeRequest { Title = initiative.Title, Description = initiative.Description, Username = initiative.Username };
            await _mailService.SendMailAsync(email, request);
            return Ok($"Ініціативу успішно підтримано користувачем {userToSend.Name}");
        }

        [HttpPatch("update-initiative/{id:int}")]
        public async Task<ActionResult<int>> UpdateInitiative([FromRoute] int id, [FromBody] InitiativeUpdateDTO initiative)
        {
            int result = await _initiativesReporitory.UpdateInitiative(id, initiative);

            if (result == 0)
            {
                return NotFound("Ініціатива не була знайдена");
            }

            return Ok("Iніціатива успішно оновлена");
        }

        [HttpDelete("delete-initiative/{id:int}")]

        public async Task<ActionResult> DeleteInitiative([FromRoute] int id)
        {
            int result  = await _initiativesReporitory.DeleteInitiative(id);
            if(result == 0)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<IEnumerable<InitiativeIdDTO>>> GetInitiativeById ([FromRoute] int id)
        {
            var result = await _initiativesReporitory.GetInitiativeById(id);

            if(result == null)
            {
                NotFound("Дана ініціатива відсутня");
            }

            return Ok(result);
        }
    }

    
}
