using System.Globalization;
using MoistSensServer;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

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

app.MapPost("/humidity-post", (HumidityData data) =>
{
    /*try
    {
        using var connection = new SqliteConnection(@"Data Source=C:\Users\simon\RiderProjects\MoistSensServer\identifier.sqlite");
        connection.Open();
        Console.WriteLine("Connected to SQLite");

        using var command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@Date", DateTime.Now);
        command.Parameters.AddWithValue("@Humidity", data.Humidity);
        command.Parameters.AddWithValue("@SensorName", data.SensorName);
        command.ExecuteNonQuery();
        
        Console.WriteLine($"{data.SensorName?.ToString(CultureInfo.InvariantCulture)} humidity: {data.Humidity.ToString(CultureInfo.InvariantCulture)} successfully added");
    }
    catch (SqliteException e)
    {
        Console.WriteLine(e.Message);
    }*/

    var postgresCreate = new PostgresCreate();
    postgresCreate.InsertHumidityData(data);

    return
        $"{DateTime.Now}: {data.SensorName?.ToString(CultureInfo.InvariantCulture)} humidity is {data.Humidity.ToString(CultureInfo.InvariantCulture)}";
})
.WithName("HumidityPost")
.WithOpenApi();
    


app.MapGet("/humidity-get", (string? name, int humidity) =>
{
    var reading = new HumidityData(name ?? throw new ArgumentNullException(nameof(name)), humidity);
    return reading;
})
.WithName("HumidityGet")
.WithOpenApi();


app.Run();
