using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pitstop.NotificationService.NotificationChannels
{
    public interface IEmailNotifier
    {
        Task SendEmailAsync(string to, string from, string subject, string body);
    }
}
