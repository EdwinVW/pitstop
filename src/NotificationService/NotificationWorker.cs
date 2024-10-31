using Pitstop.NotificationService.Message;
using Pitstop.NotificationService.Message.Templates;

namespace Pitstop.NotificationService;

public class NotificationWorker : IHostedService, IMessageHandlerCallback
{
    IMessageHandler _messageHandler;
    INotificationRepository _repo;
    IEmailNotifier _emailNotifier;
    private readonly ISlackMessenger _slackMessenger;
    private readonly IConfiguration _config;

    public NotificationWorker(IConfiguration config, IMessageHandler messageHandler, INotificationRepository repo, IEmailNotifier emailNotifier, ISlackMessenger slackMessenger)

    {
        _messageHandler = messageHandler;
        _repo = repo;
        _emailNotifier = emailNotifier;
        _slackMessenger = slackMessenger;
        _config = config;
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
                case "RepairOrderSent":
                    await HandleAsync(messageObject.ToObject<RepairOrderSent>());
                    break;
                case "RepairOrderApproved":
                    await HandleAsync(messageObject.ToObject<RepairOrderApproved>());
                    break;
                case "RepairOrderRejected":
                    await HandleAsync(messageObject.ToObject<RepairOrderRejected>());
                    break;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error while handling {messageType} event.");
        }

        return true;
    }

    private async Task HandleAsync(RepairOrderSent rs)
    {
        string baseUrl = _config.GetSection("WebLocation").GetValue<string>("WebApp");
        string subject = $"Repair Order #{rs.CustomerInfo.CustomerName} - {rs.VehicleInfo.LicenseNumber}";
        
        string repairOrderUrl = $"http://{baseUrl}/RepairManagement/DetailsCustomer/{rs.RepairOrderId}";
        
        string body = $@"
        <html>
            <body>
                <h2>Dear {rs.CustomerInfo.CustomerName},</h2>
                <p>Your repair order has been created successfully for your vehicle with license number: <strong>{rs.VehicleInfo.LicenseNumber}</strong>.</p>
                <p><strong>Vehicle Details:</strong></p>
                <ul>
                    <li>Brand: {rs.VehicleInfo.Brand}</li>
                    <li>Type: {rs.VehicleInfo.Type}</li>
                    <li>License Number: {rs.VehicleInfo.LicenseNumber}</li>
                </ul>
                <p><strong>Repair Order Details:</strong></p>
                <ul>
                    <li>Total Cost: {rs.TotalCost.ToString("C", new CultureInfo("en-US"))}</li>
                    <li>Status: {rs.Status}</li>
                </ul>
                <p>Click the button below to view and manage your repair order:</p>
                <p>
                    <a href='{repairOrderUrl}' style='padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>View Repair Order</a>
                </p>
                <br/>
                <p>Best regards,<br/>The Repair Team</p>
            </body>
        </html>";

        string email = rs.CustomerInfo.CustomerEmail;
        await _emailNotifier.SendEmailHtmlAsync(email, "info@local.com", subject, body);
    }
    
    private async Task HandleAsync(RepairOrderApproved acceptedEvent)
    {
        string subject = "Klant heeft de reparatie goedgekeurd";
        string body = $"De klant met naam {acceptedEvent.CustomerName} heeft de onderhoudstaak met ID {acceptedEvent.RepairOrderId} en kentekenplaat {acceptedEvent.LicenseNumber} goedgekeurd. U kunt nu beginnen met de reparatie.";

        await _emailNotifier.SendEmailHtmlAsync("noreply@pitstop.nl", "noreply@pitstop.nl", subject, body);

        var slackMessage = new SlackMessageBuilder()
            .AddHeader("Klant heeft de reparatie goedgekeurd")
            .AddSection($"De klant met naam {acceptedEvent.CustomerName} heeft de onderhoudstaak met ID {acceptedEvent.RepairOrderId} en kentekenplaat {acceptedEvent.LicenseNumber} goedgekeurd. U kunt nu beginnen met de reparatie.")
            .BuildSlackMessage();

        await _slackMessenger.PostMessage(slackMessage);
    }
    
    private async Task HandleAsync(RepairOrderRejected rejectedEvent)
    {
        string subject = "Klant heeft de reparatie afgewezen";
        string body = $"De klant met de naam {rejectedEvent.CustomerName} heeft de onderhoudstaak met ID {rejectedEvent.RepairOrderId} en kentekenplaat {rejectedEvent.LicenseNumber} afgewezen. Er kan geen reparatie worden uitgevoerd.";

        await _emailNotifier.SendEmailHtmlAsync("noreply@pitstop.nl", "noreply@pitstop.nl", subject, body);

        var slackMessage = new SlackMessageBuilder()
            .AddHeader("Klant heeft de reparatie afgewezen")
            .AddSection($"De klant met de naam {rejectedEvent.CustomerName} heeft de onderhoudstaak met ID {rejectedEvent.RepairOrderId} en kentekenplaat {rejectedEvent.LicenseNumber} afgewezen. Er kan geen reparatie worden uitgevoerd.")
            .BuildSlackMessage();

        await _slackMessenger.PostMessage(slackMessage);
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
        var message = new VehicleManagementFinished("Maintenance finished", $"Dear {mjf.JobId}",
        new List<string>(["Your maintenance job has finished.", "Please pickup your car"]));

        try
        {   
            Log.Information("Sending notification for finished maintenance job: {job}", mjf.JobId);
            await _slackMessenger.PostMessage(message.BuildMessage());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending notification for finished maintenance job: {job}", mjf.JobId);
        }
        
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
            body.AppendLine(
                $"We would like to remind you that you have an appointment with us for maintenance on your vehicle(s):\n");
            foreach (MaintenanceJob job in jobsPerCustomer)
            {
                body.AppendLine($"- {job.StartTime.ToString("dd-MM-yyyy")} at {job.StartTime.ToString("HH:mm")} : " +
                                $"{job.Description} on vehicle with license-number {job.LicenseNumber}");
            }

            body.AppendLine(
                $"\nPlease make sure you're present at least 10 minutes before the (first) job is planned.");
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