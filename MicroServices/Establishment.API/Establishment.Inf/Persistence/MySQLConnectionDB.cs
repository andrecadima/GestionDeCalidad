    using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Establishment.Inf.Persistence;

public class MySqlConnectionDB
{
    private readonly string connectionString;

    public MySqlConnectionDB(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
    }

    public MySqlConnection GetConnection() {
        return new MySqlConnection(connectionString);
    }
}