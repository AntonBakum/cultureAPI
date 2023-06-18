namespace CultureAPI.Controllers.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PublicationDate { get; set; }

        public string Image { get; set; }

        public string AuthorName { get; set; }
    }
}
