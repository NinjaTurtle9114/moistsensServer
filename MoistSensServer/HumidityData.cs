namespace MoistSensServer;

public class HumidityData
{
    public HumidityData(string? sensorName, int humidity)
    {
        SensorName = sensorName;
        Humidity = humidity;
    }
    public string? SensorName { get; set; }
    public int Humidity { get; set; }
}