namespace SecureSphere
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string imagePath);
    }
}
