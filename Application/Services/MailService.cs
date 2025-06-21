using CamQuizz.Application.Interfaces;

namespace CamQuizz.Application.Services;

public class MailService : IMailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // TODO: Implement actual email sending logic
        // For now, just log the email details
        await Task.Run(() => Console.WriteLine($"Email sent to {to}: {subject}"));
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml)
    {
        // TODO: Implement actual email sending logic with HTML support
        await Task.Run(() => Console.WriteLine($"Email sent to {to}: {subject} (HTML: {isHtml})"));
    }

    public async Task SendWelcomeEmailAsync(string to, string username)
    {
        var subject = "Welcome to BasedProject!";
        var body = $@"
            <h1>Welcome {username}!</h1>
            <p>Thank you for registering with BasedProject. We're excited to have you on board!</p>
            <p>You can now start creating and taking quizzes.</p>
        ";
        
        await SendEmailAsync(to, subject, body, true);
    }

    public async Task SendPasswordResetEmailAsync(string to, string resetToken)
    {
        var subject = "Password Reset Request";
        var body = $@"
            <h1>Password Reset</h1>
            <p>You have requested a password reset.</p>
            <p>Your reset token is: <strong>{resetToken}</strong></p>
            <p>If you didn't request this, please ignore this email.</p>
        ";
        
        await SendEmailAsync(to, subject, body, true);
    }
} 