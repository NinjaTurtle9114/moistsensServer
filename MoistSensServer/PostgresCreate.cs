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
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public async void InsertHumidityData(HumidityData data)
    {
        const string sqlHumidityData = "INSERT INTO humidity_table(date, humidity, sensorname)" +
                           "VALUES(@date, @humidity, @sensorname)";
        
        try
        {
            using var dataSource = NpgsqlDataSource.Create(_connectionString);

            await using var command = dataSource.CreateCommand(sqlHumidityData);
            
            var local = DateTime.Now;
            var utc = DateTime.SpecifyKind(local, DateTimeKind.Utc);

            var timeStampedHumidityData = new TimeStampedHumidityData(data, utc);
            
            command.Parameters.AddWithValue("@date", timeStampedHumidityData.TimeStamp);
            command.Parameters.AddWithValue("@humidity",timeStampedHumidityData.Data.Humidity);
            command.Parameters.AddWithValue("@sensorname", timeStampedHumidityData.Data.SensorName ?? throw new InvalidOperationException(
                "Missing sensorName. HumidityData sensorName variable cannot be null"));
            await command.ExecuteNonQueryAsync();
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public async void InsertSensorDescription(string sensorName,string sensorDescription)
    {
        const string sqlSensorName = "INSERT INTO sensor_description(sensorname, sensordescription)" +
                                     "VALUES(@sensorname, @sensordescription)";

        try
        {
            using var dataSource = NpgsqlDataSource.Create(_connectionString);
            await using var command = dataSource.CreateCommand(sqlSensorName);

            command.Parameters.AddWithValue("@sensorname", sensorName);
            command.Parameters.AddWithValue("@sensordescription", sensorDescription);
            await command.ExecuteNonQueryAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public async void QueryDescription(string description)
    {
        const string sql = @"SELECT sensorname, sensordescription
                            FROM sensor_description
                            WHERE sensordescription=@sensordescription";

        try
        {
            await using var dataSource = NpgsqlDataSource.Create(_connectionString);
            await using var command = dataSource.CreateCommand(sql);

            command.Parameters.AddWithValue("@sensordescription", description);
            
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                var sensorName = reader.GetString(0);
                var sensorDescription = reader.GetString(1);
                
                Console.WriteLine($"{sensorName}\t{sensorDescription}");
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public async void QueryHumidityData(string sensorName, DateTime? startStamp, DateTime? endStamp)
    {
        const string sql = @"SELECT date, sensorname, humidity
                            FROM humidity_table
                            WHERE date >= @startStamp AND date < @endStamp
                            AND sensorname=@sensorName";
        try
        {
            await using var dataSource = NpgsqlDataSource.Create(_connectionString);
            await using var command = dataSource.CreateCommand(sql);

            command.Parameters.AddWithValue("@sensorName", sensorName);
            if (startStamp != null) command.Parameters.AddWithValue("@startStamp", startStamp);
            if (endStamp != null) command.Parameters.AddWithValue("@endStamp", endStamp);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var date = reader.GetDateTime(0);
                var name = reader.GetString(1);
                var humidity = reader.GetInt32(2);
                
                Console.WriteLine($"{date} : Sensor {name} humidity is {humidity}");
            }

        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

    }
    
}