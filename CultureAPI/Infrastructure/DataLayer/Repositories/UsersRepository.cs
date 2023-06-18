using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Dapper;
using CultureAPI.Infrastructure.Queries;
using CultureAPI.Infrastructure.DataLayer.DataLayerModels;
using CultureAPI.Domain.ServiceAbstractions;
using CultureAPI.Domain.DataLayerAbstractions;
using static Dapper.SqlMapper;

namespace CultureAPI.Infrastructure.DataLayer.Repositories
{
    public class UsersRepository: IUsersRepository
    {
        private readonly IDatabaseContext _context;
        private readonly IAuthService _authService;

        public UsersRepository(IDatabaseContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<int> AddUser(UserRegistrationDTO entity)
        {
            string passwordSalt = _authService.GenerateRandomString(10);
            return await _context.SqlConnection.ExecuteScalarAsync<int>(UsersQueries.createUser,
                new
                {
                    entity.Name,
                    entity.Email,
                    Password = _authService.CreatePasswordHash(entity.Password, passwordSalt),
                    PasswordSalt = passwordSalt,
                });
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.SqlConnection.QuerySingleOrDefaultAsync<User>(UsersQueries.getUserByEmail, new { Email = email });
        }

        public async Task<TokenUserModel> GetUserById(int id)
        {
            return await _context.SqlConnection.QuerySingleOrDefaultAsync<TokenUserModel>(UsersQueries.getUserById, new { Id = id });
        }

        public async Task<int> UpdateUser (int id, UserUpdateDTO user)
        {
            string passwordSalt = _authService.GenerateRandomString(10);
            return await _context.SqlConnection.ExecuteAsync(UsersQueries.updateUser, 
                new { Id = id, user.Name, user.Email, user.Phone, user.Nickname,                 
                });
        }
    }
}
