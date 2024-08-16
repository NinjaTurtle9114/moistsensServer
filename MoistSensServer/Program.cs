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
})
.WithName("SetHumidity")
.WithOpenApi();

app.MapPost("/set-sensor-description", (SensorDescription sensor) =>
{
    postgresCreate.InsertSensorDescription(sensor.SensorName ?? throw new InvalidOperationException("null sensor name"),
                                            sensor.Description);
})
.WithName("SensorDescription")
.WithOpenApi();

// Get all sensor names from description, should probably have another GET for giving description of a sensor (maybe)
app.MapGet("/get-sensor-description", async (string description) =>
    {
        var data = await postgresCreate.QueryDescription(description);
        return Results.Json(data);
    })
.WithDescription("GetSensorDescription")
.WithOpenApi();

app.MapGet("/get-humidity-time-period", async (string sensorName, DateTime? start, DateTime? end) =>
{
    start ??= DateTime.Now;                 // If no date is given we assume the user wants today's HumidityData
    end ??= DateTime.Now.Date.AddDays(1);   // Might change this to query the most recent HumidityData instead of
                                            // setting start and end to Now

    var data = await postgresCreate.QueryHumidityData(sensorName, start, end);
    return Results.Json(data);
})
.WithDescription("GetHumidityTimePeriod")
.WithOpenApi();


app.Run();
