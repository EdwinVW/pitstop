namespace Pitstop.UITest;

public static class TestConstants
{
    public static Uri PitstopStartUrl => new Uri("http://localhost:7005");

    /// <summary>
    /// Delay to allow the WorkshopManagementEventHandler to process events via RabbitMQ
    /// and build its local cache of customers and vehicles.
    /// </summary>
    public static readonly TimeSpan EventProcessingDelay = TimeSpan.FromSeconds(2);
    
    /// <summary>
    /// Delay to allow the database cleanup to fully complete.
    /// </summary>
    public static readonly TimeSpan DatabaseCleanupDelay = TimeSpan.FromSeconds(1);

    public static string SqlConnectionString =>
        "Server=localhost,1434;User Id=sa;Password=8jkGh47hnDw89H@q8LN2;Encrypt=False;MultipleActiveResultSets=True";
}