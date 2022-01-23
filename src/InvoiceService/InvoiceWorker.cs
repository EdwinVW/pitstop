namespace Pitstop.InvoiceService;

public class InvoiceWorker : IHostedService, IMessageHandlerCallback
{
    private const decimal HOURLY_RATE = 18.50M;
    private IMessageHandler _messageHandler;
    private IInvoiceRepository _repo;
    private IEmailCommunicator _emailCommunicator;

    public InvoiceWorker(IMessageHandler messageHandler, IInvoiceRepository repo, IEmailCommunicator emailCommunicator)
    {
        _messageHandler = messageHandler;
        _repo = repo;
        _emailCommunicator = emailCommunicator;
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
        Log.Information("Register customer: {Id}, {Name}, {Address}, {PostalCode}, {City}",
            cr.CustomerId, cr.Name, cr.Address, cr.PostalCode, cr.City);

        Customer customer = new Customer
        {
            CustomerId = cr.CustomerId,
            Name = cr.Name,
            Address = cr.Address,
            PostalCode = cr.PostalCode,
            City = cr.City
        };

        await _repo.RegisterCustomerAsync(customer);
    }

    private async Task HandleAsync(MaintenanceJobPlanned mjp)
    {
        Log.Information("Register Maintenance Job: {Id}, {Description}, {CustomerId}, {VehicleLicenseNumber}",
            mjp.JobId, mjp.Description, mjp.CustomerInfo.Id, mjp.VehicleInfo.LicenseNumber);

        MaintenanceJob job = new MaintenanceJob
        {
            JobId = mjp.JobId.ToString(),
            CustomerId = mjp.CustomerInfo.Id,
            LicenseNumber = mjp.VehicleInfo.LicenseNumber,
            Description = mjp.Description
        };

        await _repo.RegisterMaintenanceJobAsync(job);
    }

    private async Task HandleAsync(MaintenanceJobFinished mjf)
    {
        Log.Information("Finish Maintenance Job: {Id}, {StartTime}, {EndTime}",
            mjf.JobId, mjf.StartTime, mjf.EndTime);

        await _repo.MarkMaintenanceJobAsFinished(mjf.JobId, mjf.StartTime, mjf.EndTime);
    }

    private async Task HandleAsync(DayHasPassed dhp)
    {
        var jobs = await _repo.GetMaintenanceJobsToBeInvoicedAsync();
        foreach (var jobsPerCustomer in jobs.GroupBy(job => job.CustomerId))
        {
            DateTime invoiceDate = DateTime.Now;
            string customerId = jobsPerCustomer.Key;
            Customer customer = await _repo.GetCustomerAsync(customerId);
            Invoice invoice = new Invoice
            {
                InvoiceId = $"{invoiceDate.ToString("yyyyMMddhhmmss")}-{customerId.Substring(0, 4)}",
                InvoiceDate = invoiceDate.Date,
                CustomerId = customer.CustomerId,
                JobIds = string.Join('|', jobsPerCustomer.Select(j => j.JobId))
            };

            StringBuilder specification = new StringBuilder();
            decimal totalAmount = 0;
            foreach (var job in jobsPerCustomer)
            {
                TimeSpan duration = job.EndTime.Value.Subtract(job.StartTime.Value);
                decimal amount = Math.Round((decimal)duration.TotalHours * HOURLY_RATE, 2);
                totalAmount += amount;
                specification.AppendLine($"{job.EndTime.Value.ToString("dd-MM-yyyy")} : {job.Description} on vehicle with license {job.LicenseNumber} - Duration: {duration.TotalHours} hour - Amount: &euro; {amount}");
            }
            invoice.Specification = specification.ToString();
            invoice.Amount = totalAmount;

            await SendInvoice(customer, invoice);
            await _repo.RegisterInvoiceAsync(invoice);

            Log.Information("Invoice {Id} sent to {Customer}", invoice.InvoiceId, customer.Name);
        }
    }

    private async Task SendInvoice(Customer customer, Invoice invoice)
    {
        StringBuilder body = new StringBuilder();

        // top banner
        body.AppendLine("<htm><body style='width: 1150px; font-family: Arial;'>");
        body.AppendLine("<image src='cid:banner.jpg'>");

        body.AppendLine("<table style='width: 100%; border: 0px; font-size: 25pt;'><tr>");
        body.AppendLine("<td>PITSTOP GARAGE</td>");
        body.AppendLine("<td style='text-align: right;'>INVOICE</td>");
        body.AppendLine("</tr></table>");

        body.AppendLine("<hr>");

        // invoice and customer details
        body.AppendLine("<table style='width: 100%; border: 0px;'><tr>");

        body.AppendLine("<td width='150px' valign='top'>");
        body.AppendLine("Invoice reference<br/>");
        body.AppendLine("Invoice date<br/>");
        body.AppendLine("Amount<br/>");
        body.AppendLine("Payment due by<br/>");
        body.AppendLine("</td>");

        body.AppendLine("<td valign='top'>");
        body.AppendLine($": {invoice.InvoiceId}<br/>");
        body.AppendLine($": {invoice.InvoiceDate.ToString("dd-MM-yyyy")}<br/>");
        body.AppendLine($": &euro; {invoice.Amount}<br/>");
        body.AppendLine($": {invoice.InvoiceDate.AddDays(30).ToString("dd-MM-yyyy")}<br/>");
        body.AppendLine("</td>");

        body.AppendLine("<td width='50px' valign='top'>");
        body.AppendLine("To:");
        body.AppendLine("</td>");

        body.AppendLine("<td valign='top'>");
        body.AppendLine($"{customer.Name}<br/>");
        body.AppendLine($"{customer.Address}<br/>");
        body.AppendLine($"{customer.PostalCode}<br/>");
        body.AppendLine($"{customer.City}<br/>");
        body.AppendLine("</td>");

        body.AppendLine("</tr></table>");

        body.AppendLine("<hr><br/>");

        // body
        body.AppendLine($"Dear {customer.Name},<br/><br/>");
        body.AppendLine("Hereby we send you an invoice for maintenance we executed on your vehicle(s):<br/>");

        body.AppendLine("<ol>");
        foreach (string specificationLine in invoice.Specification.Split('\n'))
        {
            if (specificationLine.Length > 0)
            {
                body.AppendLine($"<li>{specificationLine}</li>");
            }
        }
        body.AppendLine("</ol>");


        body.AppendLine($"Total amount : &euro; {invoice.Amount}<br/><br/>");

        body.AppendLine("Payment terms : Payment within 30 days of invoice date.<br/><br/>");

        // payment details
        body.AppendLine("Payment details<br/><br/>");

        body.AppendLine("<table style='width: 100%; border: 0px;'><tr>");

        body.AppendLine("<td width='120px' valign='top'>");
        body.AppendLine("Bank<br/>");
        body.AppendLine("Name<br/>");
        body.AppendLine("IBAN<br/>");
        body.AppendLine($"Reference<br/>");
        body.AppendLine("</td>");

        body.AppendLine("<td valign='top'>");
        body.AppendLine(": ING<br/>");
        body.AppendLine(": Pitstop Garage<br/>");
        body.AppendLine(": NL20INGB0001234567<br/>");
        body.AppendLine($": {invoice.InvoiceId}<br/>");
        body.AppendLine("</td>");

        body.AppendLine("</tr></table><br/>");

        // greetings
        body.AppendLine("Greetings,<br/><br/>");
        body.AppendLine("The PitStop crew<br/>");

        body.AppendLine("</htm></body>");

        MailMessage mailMessage = new MailMessage
        {
            From = new MailAddress("invoicing@pitstop.nl"),
            Subject = $"Pitstop Garage Invoice #{invoice.InvoiceId}"
        };
        mailMessage.To.Add("pitstop@prestoprint.nl");

        mailMessage.Body = body.ToString();
        mailMessage.IsBodyHtml = true;

        Attachment bannerImage = new Attachment(@"Assets/banner.jpg");
        string contentID = "banner.jpg";
        bannerImage.ContentId = contentID;
        bannerImage.ContentDisposition.Inline = true;
        bannerImage.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
        mailMessage.Attachments.Add(bannerImage);

        await _emailCommunicator.SendEmailAsync(mailMessage);
    }
}
