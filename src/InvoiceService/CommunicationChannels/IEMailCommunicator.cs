using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Pitstop.InvoiceService.CommunicationChannels
{
    public interface IEmailCommunicator
    {
        Task SendEmailAsync(MailMessage message);
    }
}
