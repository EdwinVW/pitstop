using Moq;
using Newtonsoft.Json;
using Pitstop.Infrastructure.Messaging;
using Pitstop.NotificationService;
using Pitstop.NotificationService.Events;
using Pitstop.NotificationService.Model;
using Pitstop.NotificationService.NotificationChannels;
using Pitstop.NotificationService.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NotificationService.UnitTests
{
    public class NotificationHandlerTests
    {
        [Fact]
        public async Task NotificationManager_HandleMessageAsync_Should_Handle_Customer_Updated_EventAsync()
        {
            var mockMessageHandler = new Mock<IMessageHandler>();
            var mockRepository = new Mock<INotificationRepository>();
            var mockEmailNotifier = new Mock<IEmailNotifier>();

            var customer = new CustomerUpdated(Guid.NewGuid(),
                Guid.NewGuid().ToString(), 
                "johndoe@dns.com", 
                "John Doe",
                "0798745715");

            mockRepository
                .Setup(x => x.UpdateCustomerAsync(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);

            var notifyManager = new NotificationManager(mockMessageHandler.Object, mockRepository.Object, mockEmailNotifier.Object);

            await notifyManager.HandleMessageAsync("CustomerUpdated", JsonConvert.SerializeObject(customer));

            mockRepository.Verify(x => x.UpdateCustomerAsync(It.IsAny<Customer>()));
        }
    }
}
