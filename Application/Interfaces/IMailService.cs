namespace CamQuizz.Application.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailAsync(string to, string subject, string body, bool isHtml);
    Task SendWelcomeEmailAsync(string to, string username);
    Task SendPasswordResetEmailAsync(string to, string resetToken);
} 