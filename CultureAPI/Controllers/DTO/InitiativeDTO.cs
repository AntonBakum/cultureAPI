namespace CultureAPI.Controllers.DTO
{
    public class InitiativeDTO
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string CreationDate { get; set; }

        public int? NumberOfSupporters { get; set; }

        public string Description { get; set; }
    }
}
