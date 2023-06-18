using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.DataLayerAbstractions;
using CultureAPI.Domain.Models;
using CultureAPI.Infrastructure.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using static Dapper.SqlMapper;

namespace CultureAPI.Infrastructure.DataLayer.Repositories
{
    public class RefreshTokensRepository: IRefreshTokensRepository
    {
        private readonly IDatabaseContext _context;

        public RefreshTokensRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AddRefreshToken(RefreshToken refreshToken)
        {
            return await _context.SqlConnection.ExecuteScalarAsync<int>(RefreshTokensQueries.addRefreshToken,
            new
            {
               refreshToken.JwtId,
               refreshToken.ExpiryDate,
               refreshToken.Token,
               refreshToken.isRevoked,
               refreshToken.isUsed,
               refreshToken.AddedDate,
               refreshToken.UserId,
            });
        }

        public async Task<RefreshToken> GetToken(TokenRequestDTO tokenRequest)
        {
            return await _context.SqlConnection.QueryFirstOrDefaultAsync<RefreshToken>(RefreshTokensQueries.getRefreshToken,
                new { Token = tokenRequest.RefreshToken });
        }

        public async Task<int> UpdateTokenStatus(string tokenToUpdate, bool tokenStatus)
        {
            return await _context.SqlConnection.ExecuteAsync(RefreshTokensQueries.updateTokenStatus,
                new { Token = tokenToUpdate, isUsed = tokenStatus });
        }

    }
}
