namespace Pitstop.NotificationService.NotificationChannels;

public interface IEmailNotifier
{
    Task SendEmailAsync(string to, string from, string subject, string body);
    Task SendEmailHtmlAsync(string to, string from, string subject, string body);
}