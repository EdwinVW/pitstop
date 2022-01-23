namespace Pitstop.InvoiceService.CommunicationChannels;

public interface IEmailCommunicator
{
    Task SendEmailAsync(MailMessage message);
}