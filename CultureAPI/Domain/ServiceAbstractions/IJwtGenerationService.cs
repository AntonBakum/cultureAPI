using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.Models;

namespace CultureAPI.Domain.ServiceAbstractions
{
    public interface IJwtGenerationService
    {
        Task<AuthResult> GenerateJwtToken(string email, int id);

        Task<AuthResult> ValidateAndGenerateToken(TokenRequestDTO tokenRequest);
    }
}
