using System.Globalization;
using MoistSensServer;

var builder = WebApplication.CreateBuilder(args);
var postgresCreate = new PostgresCreate();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/set-humidity", (HumidityData data) =>
{
    postgresCreate.InsertHumidityData(data);

    return
        $"{DateTime.Now}: {data.SensorName?.ToString(CultureInfo.InvariantCulture)} humidity is " +
        $"{data.Humidity.ToString(CultureInfo.InvariantCulture)}";
})
.WithName("SetHumidity")
.WithOpenApi();

app.MapPost("/set-sensor-description", (SensorDescription sensor) =>
{
    postgresCreate.InsertSensorDescription(sensor.SensorName ?? throw new InvalidOperationException("null sensor name"),
                                            sensor.Description);
    return $"{sensor.SensorName} description is: {sensor.Description}";
})
.WithName("SensorDescription")
.WithOpenApi();

//app.MapGet("/get-sensor-descriptions", )

app.MapGet("/humidity-get", (string? name, int humidity) =>
{
    var reading = new HumidityData(name ?? throw new ArgumentNullException(nameof(name)), humidity);
    return reading;
})
.WithName("HumidityGet")
.WithOpenApi();

app.Run();
