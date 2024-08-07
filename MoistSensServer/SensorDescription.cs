namespace MoistSensServer;

public class SensorDescription(string? sensorName, string description)
{
    public string? SensorName { get; set; } = sensorName;
    public string Description { get; set; } = description; // TODO : null check
}