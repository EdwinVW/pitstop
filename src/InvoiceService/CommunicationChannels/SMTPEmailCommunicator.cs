namespace Pitstop.InvoiceService.CommunicationChannels;

public class SMTPEmailCommunicator : IEmailCommunicator
{
    private string _smptServer;
    private int _smtpPort;
    private string _userName;
    private string _password;


    public SMTPEmailCommunicator(string smtpServer, int smtpPort, string userName, string password)
    {
        _smptServer = smtpServer;
        _smtpPort = smtpPort;
        _userName = userName;
        _password = password;
    }

    public async Task SendEmailAsync(MailMessage message)
    {
        using (SmtpClient client = new SmtpClient(_smptServer, _smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("_username", "_password");

            await Policy
                .Handle<Exception>()
                .WaitAndRetry(3, r => TimeSpan.FromSeconds(2), (ex, ts) => { Log.Error("Error sending mail. Retrying in 2 sec."); })
                .Execute(() => client.SendMailAsync(message))
                .ContinueWith(_ => Log.Information($"Invoice mail sent to printing company."));
        }
    }
}