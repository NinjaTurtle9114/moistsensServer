namespace MoistSensServer;

public class HumidityData(string? sensorName, int humidity)
{
    public string? SensorName { get; set; } = sensorName;
    public int Humidity { get; set; } = humidity;
}