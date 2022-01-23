namespace Pitstop.NotificationService.NotificationChannels;

public class SMTPEmailNotifier : IEmailNotifier
{
    private string _smptServer;
    private int _smtpPort;
    private string _userName;
    private string _password;


    public SMTPEmailNotifier(string smtpServer, int smtpPort, string userName, string password)
    {
        _smptServer = smtpServer;
        _smtpPort = smtpPort;
        _userName = userName;
        _password = password;
    }

    public async Task SendEmailAsync(string to, string from, string subject, string body)
    {
        using (SmtpClient client = new SmtpClient(_smptServer, _smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("_username", "_password");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from);
            mailMessage.To.Add(to);
            mailMessage.Body = body;
            mailMessage.Subject = subject;

            await Policy
                .Handle<Exception>()
                .WaitAndRetry(3, r => TimeSpan.FromSeconds(2), (ex, ts) => { Log.Error("Error sending mail. Retrying in 2 sec."); })
                .Execute(() => client.SendMailAsync(mailMessage))
                .ContinueWith(_ => Log.Information("Notification mail sent to {Recipient}.", to));
        }
    }
}