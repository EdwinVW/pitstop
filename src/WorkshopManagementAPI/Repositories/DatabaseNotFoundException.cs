namespace Pitstop.WorkshopManagementAPI.Repositories;

public class DatabaseNotCreatedException : Exception
{
    public DatabaseNotCreatedException()
    {
    }

    public DatabaseNotCreatedException(string message) : base(message)
    {
    }

    public DatabaseNotCreatedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}