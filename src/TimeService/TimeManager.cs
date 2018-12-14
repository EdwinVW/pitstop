using Pitstop.Infrastructure.Messaging;
using Pitstop.TimeService.Events;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pitstop.TimeService
{
    public class TimeManager
    {
        DateTime _lastCheck;
        CancellationTokenSource _cancellationTokenSource;
        Task _task;
        IMessagePublisher _messagePublisher;


        public TimeManager(IMessagePublisher messagePublisher)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _lastCheck = DateTime.Now;
            _messagePublisher = messagePublisher;
        }

        public void Start()
        {
            _task = Task.Run(() => Worker(), _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private async void Worker()
        {
            while (true)
            {
                if (DateTime.Now.Subtract(_lastCheck).Days > 0)
                {
                    Log.Information($"Day has passed!");
                    _lastCheck = DateTime.Now;
                    DateTime passedDay = _lastCheck.AddDays(-1);
                    DayHasPassed e = new DayHasPassed(Guid.NewGuid());
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
                }
                Thread.Sleep(10000);
            }
        }
    }
}
