namespace CultureAPI.Domain.Models
{
    public class News
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } 

        public string Content { get; set; }

        public string PublicationDate { get; set; }

        public string Image { get; set; }
    }
}
