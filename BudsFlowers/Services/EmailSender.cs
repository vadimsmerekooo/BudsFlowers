using MimeKit;
using MailKit.Net.Smtp;

namespace BudsFlowers.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                using var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("Интернет-магазин цветов", "budsflowers.grodno@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true);
                    await client.AuthenticateAsync("budsflowers.grodno@gmail.com", "oczt erov hdsn ynme");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch
            {

            }
        }
    }
}
