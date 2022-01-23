namespace Pitstop.WorkshopManagementAPI.Domain.Exceptions;

public class InvalidValueException : Exception
{
    public InvalidValueException()
    {
    }

    public InvalidValueException(string message) : base(message)
    {
    }

    public InvalidValueException(string message, Exception innerException) : base(message, innerException)
    {
    }
}