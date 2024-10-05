using System.Net;
using System.Net.Mail;
namespace SecureSphere
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message, string imagePath)
        {
            var mail = "SecureSpherealert@gmail.com";
            var password = "vmfb zgnb tdip metb";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message,
            };

            mailMessage.To.Add(email);

            // Check if the image file exists
            if (File.Exists(imagePath))
            {
                try
                {
                    // Create the attachment
                    Attachment attachment = new Attachment(imagePath);

                    // Add the attachment to the message
                    mailMessage.Attachments.Add(attachment);
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur while attaching the file
                    throw new Exception($"Error attaching file: {ex.Message}");
                }
            }
            else
            {
                throw new FileNotFoundException($"The specified image file was not found: {imagePath}");
            }

            return client.SendMailAsync(mailMessage);
        }
    }
}