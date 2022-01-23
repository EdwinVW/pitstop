namespace Pitstop.WorkshopManagementAPI.Domain.Exceptions;

public class MaintenanceJobNotFoundException : Exception
{
    public MaintenanceJobNotFoundException()
    {
    }

    public MaintenanceJobNotFoundException(string message) : base(message)
    {
    }

    public MaintenanceJobNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}