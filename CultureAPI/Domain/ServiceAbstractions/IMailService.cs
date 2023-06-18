using CultureAPI.Domain.Models;

namespace CultureAPI.Domain.ServiceAbstractions
{
    public interface IMailService
    {
        Task SendMailAsync (Email email, InitiativeRequest request);
    }
}
