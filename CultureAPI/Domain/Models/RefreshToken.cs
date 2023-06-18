namespace CultureAPI.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool isUsed { get; set; }

        public bool isRevoked { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
