using Newtonsoft.Json.Linq;
using Pitstop.Infrastructure.Messaging;
using Pitstop.NotificationService.Events;
using Pitstop.NotificationService.Model;
using Pitstop.NotificationService.NotificationChannels;
using Pitstop.NotificationService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Pitstop.NotificationService
{
    public class NotificationManager : IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        INotificationRepository _repo;
        IEmailNotifier _emailNotifier;

        public NotificationManager(IMessageHandler messageHandler, INotificationRepository repo, IEmailNotifier emailNotifier)
        {
            _messageHandler = messageHandler;
            _repo = repo;
            _emailNotifier = emailNotifier;
        }

        public void Start()
        {
            _messageHandler.Start(this);
        }

        public void Stop()
        {
            _messageHandler.Stop();
        }

        public async Task<bool> HandleMessageAsync(MessageTypes messageType, string message)
        {
            JObject messageObject = MessageSerializer.Deserialize(message);
            switch (messageType)
            {
                case MessageTypes.CustomerRegistered:
                    await HandleAsync(messageObject.ToObject<CustomerRegistered>());
                    break;
                case MessageTypes.MaintenanceJobPlanned:
                    await HandleAsync(messageObject.ToObject<MaintenanceJobPlanned>());
                    break;
                case MessageTypes.MaintenanceJobFinished:
                    await HandleAsync(messageObject.ToObject<MaintenanceJobFinished>());
                    break;
                case MessageTypes.DayHasPassed:
                    await HandleAsync(messageObject.ToObject<DayHasPassed>());
                    break;
                default:
                    break;
            }
            return true;
        }

        private async Task HandleAsync(CustomerRegistered cr)
        {
            Customer customer = new Customer
            {
                CustomerId = cr.CustomerId,
                Name = cr.Name,
                TelephoneNumber = cr.TelephoneNumber,
                EmailAddress = cr.EmailAddress
            };

            await _repo.RegisterCustomerAsync(customer);
        }

        private async Task HandleAsync(MaintenanceJobPlanned mjp)
        {
            MaintenanceJob job = new MaintenanceJob
            {
                JobId = mjp.JobId.ToString(),
                CustomerId = mjp.CustomerInfo.Id,
                LicenseNumber = mjp.VehicleInfo.LicenseNumber,
                StartTime = mjp.StartTime,
                Description = mjp.Description
            };

            await _repo.RegisterMaintenanceJobAsync(job);
        }

        private async Task HandleAsync(MaintenanceJobFinished mjf)
        {
            await _repo.RemoveMaintenanceJobsAsync(new string[] { mjf.JobId.ToString() });
        }

        private async Task HandleAsync(DayHasPassed dhp)
        {
            DateTime today = DateTime.Now;

            IEnumerable<MaintenanceJob> jobsToNotify = await _repo.GetMaintenanceJobsForTodayAsync(today);
            foreach (var jobsPerCustomer in jobsToNotify.GroupBy(job => job.CustomerId))
            {
                // build notification body
                string customerId = jobsPerCustomer.Key;
                Customer customer = await _repo.GetCustomerAsync(customerId);
                StringBuilder body = new StringBuilder();
                body.AppendLine($"Dear {customer.Name},\n");
                body.AppendLine($"We would like to remind you that you have an appointment with us for maintenance on your vehicle(s):\n");
                foreach (MaintenanceJob job in jobsToNotify)
                {
                    body.AppendLine($"- {job.StartTime.ToString("dd-MM-yyyy")} at {job.StartTime.ToString("HH:mm")} : " +
                        $"{job.Description} on vehicle with license-number {job.LicenseNumber}");
                }

                body.AppendLine($"\nPlease make sure you're present at least 10 minutes before the (first) job is planned.");
                body.AppendLine($"Once arrived, you can notify your arrival at our front-desk.\n");
                body.AppendLine($"Greetings,\n");
                body.AppendLine($"The PitStop crew");

                // sent notification
                await _emailNotifier.SendEmailAsync(
                    customer.EmailAddress, "noreply@pitstop.nl", "Vehicle maintenance reminder", body.ToString());

                // remove jobs for which a notification was sent
                await _repo.RemoveMaintenanceJobsAsync(jobsPerCustomer.Select(job => job.JobId));
            }
        }
    }
}
