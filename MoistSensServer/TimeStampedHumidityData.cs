﻿namespace MoistSensServer;

public class TimeStampedHumidityData(HumidityData data, DateTime timeStamp)
{
    public HumidityData Data { get; set; } = data;
    public DateTime TimeStamp { get; set; } = timeStamp;
}