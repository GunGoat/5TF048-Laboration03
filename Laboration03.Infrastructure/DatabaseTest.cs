using Laboration03.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Infrastructure;

public class DatabaseTest
{
    private readonly string _connectionString;

    public DatabaseTest(IConfiguration configuration)
    {
        // Fetch the connection string from appsettings.json
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public bool CanConnectToDatabase()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                // If connection is successful, return true
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}

