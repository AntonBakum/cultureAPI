using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.Models;
using CultureAPI.Infrastructure.DataLayer.DataLayerModels;

namespace CultureAPI.Domain.DataLayerAbstractions
{
    public interface IUsersRepository
    {
        Task<int> AddUser(UserRegistrationDTO entity);

        Task<User> GetUserByEmail(string email);

        Task<TokenUserModel> GetUserById(int id);

        Task<int> UpdateUser(int id, UserUpdateDTO user);
    }
}
