namespace CultureAPI.Domain.Models
{
    public enum EmailType
    {
       Support,
       Registration
    }

    public class Email
    {
        public string ToEmail { get; set; }

        public string Template { get; set; }

        public List<IFormFile>? Attachments { get; set; }

        public EmailType EmailType { get; set;}
    }
}
