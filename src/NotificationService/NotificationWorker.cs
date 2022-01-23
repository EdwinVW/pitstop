namespace Pitstop.NotificationService;

public class NotificationWorker : IHostedService, IMessageHandlerCallback
{
    IMessageHandler _messageHandler;
    INotificationRepository _repo;
    IEmailNotifier _emailNotifier;

    public NotificationWorker(IMessageHandler messageHandler, INotificationRepository repo, IEmailNotifier emailNotifier)
    {
        _messageHandler = messageHandler;
        _repo = repo;
        _emailNotifier = emailNotifier;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Start(this);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Stop();
        return Task.CompletedTask;
    }

    public async Task<bool> HandleMessageAsync(string messageType, string message)
    {
        try
        {
            JObject messageObject = MessageSerializer.Deserialize(message);
            switch (messageType)
            {
                case "CustomerRegistered":
                    await HandleAsync(messageObject.ToObject<CustomerRegistered>());
                    break;
                case "MaintenanceJobPlanned":
                    await HandleAsync(messageObject.ToObject<MaintenanceJobPlanned>());
                    break;
                case "MaintenanceJobFinished":
                    await HandleAsync(messageObject.ToObject<MaintenanceJobFinished>());
                    break;
                case "DayHasPassed":
                    await HandleAsync(messageObject.ToObject<DayHasPassed>());
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error while handling {messageType} event.");
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

        Log.Information("Register customer: {Id}, {Name}, {TelephoneNumber}, {Email}",
            customer.CustomerId, customer.Name, customer.TelephoneNumber, customer.EmailAddress);

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

        Log.Information("Register Maintenance Job: {Id}, {CustomerId}, {VehicleLicenseNumber}, {StartTime}, {Description}",
            job.JobId, job.CustomerId, job.LicenseNumber, job.StartTime, job.Description);

        await _repo.RegisterMaintenanceJobAsync(job);
    }

    private async Task HandleAsync(MaintenanceJobFinished mjf)
    {
        Log.Information("Remove finished Maintenance Job: {Id}", mjf.JobId);

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
            foreach (MaintenanceJob job in jobsPerCustomer)
            {
                body.AppendLine($"- {job.StartTime.ToString("dd-MM-yyyy")} at {job.StartTime.ToString("HH:mm")} : " +
                    $"{job.Description} on vehicle with license-number {job.LicenseNumber}");
            }

            body.AppendLine($"\nPlease make sure you're present at least 10 minutes before the (first) job is planned.");
            body.AppendLine($"Once arrived, you can notify your arrival at our front-desk.\n");
            body.AppendLine($"Greetings,\n");
            body.AppendLine($"The PitStop crew");

            Log.Information("Sent notification to: {CustomerName}", customer.Name);

            // send notification
            await _emailNotifier.SendEmailAsync(
                customer.EmailAddress, "noreply@pitstop.nl", "Vehicle maintenance reminder", body.ToString());

            // remove jobs for which a notification was sent
            await _repo.RemoveMaintenanceJobsAsync(jobsPerCustomer.Select(job => job.JobId));
        }
    }
}