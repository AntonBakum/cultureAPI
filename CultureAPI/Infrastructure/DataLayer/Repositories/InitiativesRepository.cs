using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.DataLayerAbstractions;
using CultureAPI.Domain.Models;
using CultureAPI.Infrastructure.Queries;
using Dapper;
using System.Data;
using System.Transactions;

namespace CultureAPI.Infrastructure.DataLayer.Repositories
{
    public class InitiativesRepository: IInitiativesReporitory
    {
        private readonly IDatabaseContext _context;
        private readonly ILogger<InitiativesRepository> _logger;

        public InitiativesRepository (IDatabaseContext connection, ILogger<InitiativesRepository> logger)
        {
            _context = connection;
            _logger = logger;
        }
        public async Task<IEnumerable<Initiative>> GetAllInitiatives()
        {
            return await _context.SqlConnection.QueryAsync<Initiative>(InitiativesQueries.getAllInitiatives);
        }

        public async Task<int> CreateInitiative(InitiativeDTO initiative)
        {
            int newId = await _context.SqlConnection.ExecuteScalarAsync<int>(InitiativesQueries.addInitiative, new
            {
                initiative.Username,
                initiative.UserId,
                initiative.Title,
                initiative.CreationDate,
                initiative.NumberOfSupporters,
                initiative.Description,
            });
            return newId;
        }

        public async Task<IEnumerable<Initiative>> GetTopInitiatives()
        {
            return await _context.SqlConnection.QueryAsync<Initiative>(InitiativesQueries.getTopInitiatives);
        }

        public async Task SupportInitiative(int id, SupportInitiativeDTO supportInitiative)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _context.SqlConnection.ExecuteAsync(
                        UsersInitiativesQueries.
                        AddSupportedInitiative, new { InitiativeId = id, supportInitiative.UserId }).
                        ConfigureAwait(false);
                    await _context.SqlConnection.
                         ExecuteAsync(InitiativesQueries.updateNumberOfSupporters, new { Id = id, supportInitiative.NumberOfSupporters })
                         .ConfigureAwait(false);
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                _logger.LogError($"{DateTime.Now} ; {ex.Message}");
                throw;
            }
        }

        public async Task<int> CheckSupportedInitiative(int initiativeId, int userId)
        {
            return await _context.SqlConnection.QuerySingleAsync<int>(UsersInitiativesQueries.CheckSupportedInitiative,
                new { InitiativeId = initiativeId, UserId = userId });
        }

        public async Task<Initiative> GetInitiativeForEmail(int initiativeId)
        {
            return await _context.SqlConnection.QuerySingleAsync<Initiative>(InitiativesQueries.getInitiativeForEmail, new { Id = initiativeId });
        }

        public async Task<int> UpdateInitiative(int initiativeId, InitiativeUpdateDTO initiative)
        {
            return await _context.SqlConnection.ExecuteAsync(InitiativesQueries.updateInitiative,
                new { Id = initiativeId, initiative.Title, initiative.Description, });
        }

        public async Task<int> DeleteInitiative (int id)
        {
            return await _context.SqlConnection.ExecuteAsync(InitiativesQueries.deleteInitiative,
                new { Id = id });
        }

        public async Task<IEnumerable<InitiativeIdDTO>> GetInitiativeById (int id)
        {
            return await _context.SqlConnection.QueryAsync<InitiativeIdDTO>(InitiativesQueries.getInitiativeById, new { Id = id });
        }
    }
}
