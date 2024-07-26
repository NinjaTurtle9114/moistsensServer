using System;
using System.IO;
using MoistSensServer.Exceptions;
using Npgsql;
namespace MoistSensServer;



public class PostgresCreate
{
    private readonly string _connectionString;
    public PostgresCreate()
    {
        
        var hostString = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var host = hostString ?? throw new MissingConfigurationFieldException(
            "Missing server string. Set the POSTGRES_SERVER environment variable.");

        var userString = Environment.GetEnvironmentVariable("POSTGRES_SERVER_USER");
        var user = userString ??
                   throw new MissingConfigurationFieldException(
                       "Missing user string. Set the POSTGRES_SERVER_USER environment variable.");

        const string dbName = "postgres";
        
        const int port = 5432;

        var passwordString = Environment.GetEnvironmentVariable("POSTGRES_SERVER_PASSWORD");
        var password = passwordString ??
                       throw new MissingConfigurationFieldException(
                           "Missing password string. Set the POSTGRES_SERVER_PASSWORD environment variable");
        
        _connectionString =
            $"Host={host};Port={port};Database={dbName};Username={user};Password={password}";
        
        try
        {
            using var npgsqlConnection = new NpgsqlConnection(_connectionString);
            npgsqlConnection.Open();
        }
        catch (NpgsqlException e)
        {
            
            Console.WriteLine(e);
            throw;
        }
    }

    public async void InsertHumidityData(HumidityData data)
    {
        const string sql = "INSERT INTO humidity_table (date, humidity, sensorname)" +
                           "VALUES(@date, @humidity, @sensorname)";

        try
        {
            using var dataSource = NpgsqlDataSource.Create(_connectionString);

            await using var command = dataSource.CreateCommand(sql);

            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@humidity",data.Humidity);
            command.Parameters.AddWithValue("@sensorname", data.SensorName ?? throw new InvalidOperationException(
                "Missing sensorName. HumidityData sensorName variable cannot be null"));
            await command.ExecuteNonQueryAsync();
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            throw;
        }
    }
    
}