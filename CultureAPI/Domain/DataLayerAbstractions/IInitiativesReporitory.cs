using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.Models;

namespace CultureAPI.Domain.DataLayerAbstractions
{
    public interface IInitiativesReporitory
    {
        Task<IEnumerable<Initiative>> GetAllInitiatives();

        Task<int> CreateInitiative(InitiativeDTO initiative);

        Task<IEnumerable<Initiative>> GetTopInitiatives();

        Task SupportInitiative(int id, SupportInitiativeDTO supportInitiative);

        Task<int> CheckSupportedInitiative(int initiativeId, int userId);

        Task<Initiative> GetInitiativeForEmail(int initiativeId);

        Task<int> UpdateInitiative (int initiativeId, InitiativeUpdateDTO initiative);

        Task<int> DeleteInitiative(int id);

        Task<IEnumerable<InitiativeIdDTO>> GetInitiativeById(int id);

    }
}
