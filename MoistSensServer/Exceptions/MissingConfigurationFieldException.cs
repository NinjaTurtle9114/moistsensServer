namespace MoistSensServer.Exceptions;
/// <summary>
/// Exception to use when field is missing in the server configuration
/// </summary>
public class MissingConfigurationFieldException : ConfigurationException
{
    public MissingConfigurationFieldException(string message) : base(message)
    {
    }

    public MissingConfigurationFieldException(string message, Exception innerException) : base(message, innerException)
    {
    }
}