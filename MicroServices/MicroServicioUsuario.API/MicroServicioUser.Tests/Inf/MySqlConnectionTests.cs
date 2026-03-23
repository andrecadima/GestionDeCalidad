using MicroServicoUser.Inf.Persistence;
using MicroServicoUser.Inf.Persistence.Database;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class MySqlConnectionTests
{
    [Fact]
    public void MySqlConnectionDB_ShouldThrow_WhenConnectionStringMissing()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        Assert.Throws<InvalidOperationException>(() => new MySqlConnectionDB(config));
    }

    [Fact]
    public void MySqlConnectionDB_ShouldReturnConnection_WhenConfigured()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:MysqlUserServicioDB"] = "Server=localhost;Database=test;Uid=test;Pwd=test;"
            })
            .Build();

        var db = new MySqlConnectionDB(config);
        var conn = db.GetConnection();

        Assert.IsType<MySqlConnection>(conn);
    }

    [Fact]
    public void MySqlConnectionManager_ShouldThrow_WhenConnectionStringMissing()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        Assert.Throws<InvalidOperationException>(() => new MySqlConnectionManager(config));
    }

    [Fact]
    public void MySqlConnectionManager_ExecuteNonQuery_ShouldThrow_WhenConnectionInvalid()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:MysqlUserServicioDB"] =
                    "Server=127.0.0.1;Port=1;Database=test;Uid=test;Pwd=test;Connection Timeout=1;"
            })
            .Build();

        var manager = new MySqlConnectionManager(config);

        Assert.ThrowsAny<Exception>(() => manager.ExecuteNonQuery("SELECT 1;"));
    }
}