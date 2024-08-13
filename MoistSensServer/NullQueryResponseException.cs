namespace MoistSensServer;

public class NullQueryResponseException : ApplicationException
{
    public NullQueryResponseException(string message) : base(message)
    {
        
    }

    public NullQueryResponseException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}