using System;
using Npgsql;
namespace MoistSensServer;


public class PostgresCreate
{
    private static string Server = "localhost";
    private static string User = "postgres";
    private static string Database = "PostgreDB";
    private static string Port = "5432";
    private static string Password = "si_so*LtYoFR2dDFy@-s";

    static void Main(string[] args)
    {
        string connectionString = String.Format(
            "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
            Server,
            User,
            Database,
            Port,
            Password);
    }
}