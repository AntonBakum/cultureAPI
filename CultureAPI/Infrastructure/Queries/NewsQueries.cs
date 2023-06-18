using CultureAPI.Domain.Models;

namespace CultureAPI.Infrastructure.Queries
{
    public static class NewsQueries
    {
        public static string createNews = @"INSERT INTO News (UserId, Title, Content, PublicationDate, Image)
              OUTPUT INSERTED.Id VALUES (@UserId, @Title, @Content, @PublicationDate, @Image)";

        public static string getNews = @"SELECT News.Id, UserId, Title, Content, PublicationDate, Image, Users.Name as AuthorName
            FROM News JOIN Users ON  News.UserId = Users.Id
            ORDER BY PublicationDate DESC, News.Id DESC
            OFFSET @Number ROWS FETCH NEXT @PageSize ROWS ONLY"
        ;

        public static string getPostById = @"SELECT News.Id, UserId, Title, Content, PublicationDate, Image, Users.Name as AuthorName
            FROM News JOIN Users ON  News.UserId = Users.Id WHERE News.Id = @Id";
    }
}
