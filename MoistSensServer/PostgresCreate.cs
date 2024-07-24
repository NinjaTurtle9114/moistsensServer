using System;
using System.IO;
using MoistSensServer.Exceptions;
using Npgsql;
namespace MoistSensServer;



public class PostgresCreate
{ 
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
        
        const string port = "5432";

        var passwordString = Environment.GetEnvironmentVariable("POSTGRES_SERVER_PASSWORD");
        var password = passwordString ??
                       throw new MissingConfigurationFieldException(
                           "Missing password string. Set the POSTGRES_SERVER_PASSWORD environment variable");
        
        
        var connectionString =
            $"Host={host};Username={user};Database={dbName};Port={port};Password={password}";

        using var npgsqlConnection = new NpgsqlConnection(connectionString);
        
        npgsqlConnection.Open();
    }

    public void WriteHumidity()
    {
        var sql ="INSERT INTO humidity_table (date, humidity, sensorname)" +
                 "VALUES(@date, @humidity, @sensorname)";
    }
    
}