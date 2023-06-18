namespace CultureAPI.Domain.Models
{
    public class Initiative
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Title { get; set; }

        public string CreationDate { get; set; }

        public int NumberOfSupporters { get; set; }

        public string Description { get; set; }
    }
}
