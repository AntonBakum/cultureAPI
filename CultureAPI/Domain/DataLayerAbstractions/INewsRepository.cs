using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.Models;
using CultureAPI.Infrastructure.DataLayer;

namespace CultureAPI.Domain.DataLayerAbstractions
{
    public interface INewsRepository
    {
        Task<int> CreateNews(NewsModel entity);

        Task<IEnumerable<PostDTO>> GetNews(NewsPaginationParameters parameters, string? searchString = null);

        Task<PostDTO> GetPostById(int id);
    }
}
