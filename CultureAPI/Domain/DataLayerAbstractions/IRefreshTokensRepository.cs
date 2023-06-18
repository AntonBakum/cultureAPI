using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.Models;

namespace CultureAPI.Domain.DataLayerAbstractions
{
    public interface IRefreshTokensRepository
    {
        Task<int> AddRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> GetToken(TokenRequestDTO tokenRequest);

        Task<int> UpdateTokenStatus(string tokenToUpdate, bool tokenStatus);
    }
}
