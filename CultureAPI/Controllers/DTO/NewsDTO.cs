namespace CultureAPI.Controllers.DTO
{
    public class NewsDTO
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PublicationDate { get; set; }

        public IFormFile? Image { get; set; }
    }
}
