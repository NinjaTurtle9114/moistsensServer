namespace MoistSensServer;

public class HumidityData(string? sensorName, double humidity)
{
    public string? SensorName { get; set; } = sensorName;
    public double Humidity { get; set; } = humidity;
}