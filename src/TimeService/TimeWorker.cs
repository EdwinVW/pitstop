namespace Pitstop.TimeService;

public class TimeWorker : IHostedService
{
    DateTime _lastCheck;
    CancellationTokenSource _cancellationTokenSource;
    Task _task;
    IMessagePublisher _messagePublisher;

    public TimeWorker(IMessagePublisher messagePublisher)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _lastCheck = DateTime.Now;
        _messagePublisher = messagePublisher;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _task = Task.Run(() => Worker(), _cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
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