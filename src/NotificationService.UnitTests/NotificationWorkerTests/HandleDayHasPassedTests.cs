namespace NotificationService.UnitTests.NotificationWorkerTests;

[TestClass]
public class HandleDayHasPassedTests
{
    [TestMethod]
    public async Task HandleMessage_With_DayHasPassed_And_No_Jobs_Today_Should_Not_Send_Any_Notification()
    {
        // arrange
        string json = JsonConvert.SerializeObject(new DayHasPassedEventBuilder().Build());

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<INotificationRepository> repoMock = new Mock<INotificationRepository>();
        Mock<IEmailNotifier> emailMock = new Mock<IEmailNotifier>();

        repoMock
            .Setup(r => r.GetMaintenanceJobsForTodayAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(Enumerable.Empty<MaintenanceJob>());

        NotificationWorker sut = new NotificationWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("DayHasPassed", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        emailMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task HandleMessage_With_DayHasPassed_And_One_Job_Should_Send_Notification_And_Remove_Job()
    {
        // arrange
        string json = JsonConvert.SerializeObject(new DayHasPassedEventBuilder().Build());

        CustomerBuilder customerBuilder = new CustomerBuilder();
        Customer customer = customerBuilder.Build();

        MaintenanceJob job = new MaintenanceJobBuilder()
            .WithCustomerId(customer.CustomerId)
            .WithStartTime(DateTime.Today.AddHours(8))
            .Build();

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<INotificationRepository> repoMock = new Mock<INotificationRepository>();
        Mock<IEmailNotifier> emailMock = new Mock<IEmailNotifier>();

        repoMock
            .Setup(r => r.GetMaintenanceJobsForTodayAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(new[] { job });

        repoMock
            .Setup(r => r.GetCustomerAsync(It.Is<string>(id => id == customer.CustomerId)))
            .ReturnsAsync(customer);

        repoMock
            .Setup(r => r.RemoveMaintenanceJobsAsync(
                It.Is<IEnumerable<string>>(ids => ids.Contains(job.JobId))))
            .Returns(Task.CompletedTask);

        emailMock
            .Setup(e => e.SendEmailAsync(
                It.Is<string>(to => to == customer.EmailAddress),
                It.Is<string>(from => from == "noreply@pitstop.nl"),
                It.Is<string>(subject => subject == "Vehicle maintenance reminder"),
                It.Is<string>(body =>
                    body.Contains(customer.Name) &&
                    body.Contains(job.LicenseNumber) &&
                    body.Contains(job.Description))))
            .Returns(Task.CompletedTask);

        NotificationWorker sut = new NotificationWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("DayHasPassed", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        emailMock.VerifyAll();
        emailMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task HandleMessage_With_DayHasPassed_And_Multiple_Jobs_For_Same_Customer_Should_Send_One_Notification()
    {
        // arrange
        string json = JsonConvert.SerializeObject(new DayHasPassedEventBuilder().Build());

        CustomerBuilder customerBuilder = new CustomerBuilder();
        Customer customer = customerBuilder.Build();

        MaintenanceJob job1 = new MaintenanceJobBuilder()
            .WithCustomerId(customer.CustomerId)
            .WithStartTime(DateTime.Today.AddHours(8))
            .Build();

        MaintenanceJob job2 = new MaintenanceJobBuilder()
            .WithCustomerId(customer.CustomerId)
            .WithStartTime(DateTime.Today.AddHours(13))
            .Build();

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<INotificationRepository> repoMock = new Mock<INotificationRepository>();
        Mock<IEmailNotifier> emailMock = new Mock<IEmailNotifier>();

        repoMock
            .Setup(r => r.GetMaintenanceJobsForTodayAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(new[] { job1, job2 });

        repoMock
            .Setup(r => r.GetCustomerAsync(It.Is<string>(id => id == customer.CustomerId)))
            .ReturnsAsync(customer);

        repoMock
            .Setup(r => r.RemoveMaintenanceJobsAsync(
                It.Is<IEnumerable<string>>(ids =>
                    ids.Contains(job1.JobId) && ids.Contains(job2.JobId))))
            .Returns(Task.CompletedTask);

        emailMock
            .Setup(e => e.SendEmailAsync(
                It.Is<string>(to => to == customer.EmailAddress),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<string>(body =>
                    body.Contains(job1.LicenseNumber) &&
                    body.Contains(job2.LicenseNumber))))
            .Returns(Task.CompletedTask);

        NotificationWorker sut = new NotificationWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("DayHasPassed", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        // Only one email for both jobs
        emailMock.VerifyAll();
        emailMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task HandleMessage_With_DayHasPassed_And_Jobs_For_Different_Customers_Should_Send_Separate_Notifications()
    {
        // arrange
        string json = JsonConvert.SerializeObject(new DayHasPassedEventBuilder().Build());

        Customer customer1 = new CustomerBuilder().Build();
        Customer customer2 = new CustomerBuilder().Build();

        MaintenanceJob job1 = new MaintenanceJobBuilder()
            .WithCustomerId(customer1.CustomerId)
            .WithStartTime(DateTime.Today.AddHours(8))
            .Build();

        MaintenanceJob job2 = new MaintenanceJobBuilder()
            .WithCustomerId(customer2.CustomerId)
            .WithStartTime(DateTime.Today.AddHours(9))
            .Build();

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<INotificationRepository> repoMock = new Mock<INotificationRepository>();
        Mock<IEmailNotifier> emailMock = new Mock<IEmailNotifier>();

        repoMock
            .Setup(r => r.GetMaintenanceJobsForTodayAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(new[] { job1, job2 });

        repoMock
            .Setup(r => r.GetCustomerAsync(It.Is<string>(id => id == customer1.CustomerId)))
            .ReturnsAsync(customer1);

        repoMock
            .Setup(r => r.GetCustomerAsync(It.Is<string>(id => id == customer2.CustomerId)))
            .ReturnsAsync(customer2);

        repoMock
            .Setup(r => r.RemoveMaintenanceJobsAsync(
                It.Is<IEnumerable<string>>(ids => ids.Contains(job1.JobId))))
            .Returns(Task.CompletedTask);

        repoMock
            .Setup(r => r.RemoveMaintenanceJobsAsync(
                It.Is<IEnumerable<string>>(ids => ids.Contains(job2.JobId))))
            .Returns(Task.CompletedTask);

        emailMock
            .Setup(e => e.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        NotificationWorker sut = new NotificationWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("DayHasPassed", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        // Two separate emails: one per customer
        emailMock.Verify(e => e.SendEmailAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Exactly(2));
        emailMock.VerifyNoOtherCalls();
    }
}
