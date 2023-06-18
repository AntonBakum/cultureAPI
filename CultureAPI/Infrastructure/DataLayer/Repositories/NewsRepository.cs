using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.DataLayerAbstractions;
using CultureAPI.Domain.Models;
using CultureAPI.Infrastructure.Queries;
using Dapper;

namespace CultureAPI.Infrastructure.DataLayer.Repositories
{
    public class NewsRepository: INewsRepository
    {
        private readonly IDatabaseContext _context;

        public NewsRepository (IDatabaseContext databaseContext)
        {
            _context = databaseContext;
        }
        public async Task<int> CreateNews (NewsModel entity)
        {
            return await _context.SqlConnection.ExecuteScalarAsync<int>(NewsQueries.createNews, 
            new { 
             entity.UserId,
             entity.Content,
             entity.Title,
             entity.PublicationDate,
             entity.Image,   
            });

        }

        public async Task<IEnumerable<PostDTO>> GetNews(NewsPaginationParameters parameters, string? searchString = null)
        {
            int number = (parameters.Page - 1) * parameters.PageSize;

            var news =  await _context.SqlConnection.QueryAsync<PostDTO>(NewsQueries.getNews,
               new { PageSize = parameters.PageSize, Number = number });

            if (String.IsNullOrEmpty(searchString))
            {
                return news;
            }

            return news.Where((n) => n.Title.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));

        }

        public async Task<PostDTO> GetPostById(int id)
        {
            return await _context.SqlConnection.QuerySingleOrDefaultAsync<PostDTO>(NewsQueries.getPostById, new { Id = id });
        }
    }

}
