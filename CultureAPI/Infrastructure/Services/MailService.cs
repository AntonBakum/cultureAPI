using CultureAPI.Domain.Models;
using CultureAPI.Domain.ServiceAbstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using MimeKit;

namespace CultureAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        private string GetInitiativeMailTemplate(string templatePath, InitiativeRequest request)
        {
            StreamReader templateReader = new StreamReader(templatePath);
            string messageText = templateReader.ReadToEnd();
            templateReader.Close();
            messageText = messageText.Replace("[Username]", request.Username)
                .Replace("[Title]", request.Description)
                .Replace("[Description]", request.Title);
            return messageText;
        }

        private string GetRegistrationTemplate(string templatePath, InitiativeRequest request)
        {
            StreamReader templateReader = new StreamReader(templatePath);
            string messageText = templateReader.ReadToEnd();
            templateReader.Close();
            messageText = messageText.Replace("[Username]", request.Username)
                .Replace("[Email]", request.Description);
            return messageText;
        }

        private string TemplatePathBuilder(string template) => Path.Combine(Directory.GetCurrentDirectory(), $"Templates//{template}.html");

        public async Task SendMailAsync(Email mailRequest, InitiativeRequest request)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = $"{mailRequest.Template} {request.Title} {request.Username}";
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            string test = TemplatePathBuilder(mailRequest.Template);
            builder.HtmlBody = GetInitiativeMailTemplate(TemplatePathBuilder(mailRequest.Template), request);
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
