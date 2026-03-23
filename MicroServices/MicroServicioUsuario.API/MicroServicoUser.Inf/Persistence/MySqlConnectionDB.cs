using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicoUser.Inf.Persistence
{
    public class MySqlConnectionDB
    {
        private readonly string connectionString;

        public MySqlConnectionDB(IConfiguration configuration)
        {
<<<<<<< HEAD
            connectionString = configuration.GetConnectionString("MysqlUserServicioDB")
                ?? throw new InvalidOperationException("Connection string 'MysqlUserServicioDB' no está configurada.");
=======
            connectionString = configuration.GetConnectionString("MysqlUserServicioDB");
>>>>>>> AnalisisSonarEstablishment
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
