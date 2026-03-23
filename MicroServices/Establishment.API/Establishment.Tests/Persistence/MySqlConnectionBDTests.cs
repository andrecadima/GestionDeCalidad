using Establishment.Inf.Persistence;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Establishment.Tests.Persistence;

public class MySqlConnectionDBTests
{
    [Fact]
    public void GetConnection_Should_Return_MySqlConnection_With_Configured_ConnectionString()
    {
        // Arrange
        const string expectedConnectionString = "Server=localhost;Database=testdb;Uid=root;Pwd=1234;";

        var inMemorySettings = new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = expectedConnectionString
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var db = new MySqlConnectionDB(configuration);

        // Act
        var connection = db.GetConnection();

        // Assert
        Assert.NotNull(connection);
        Assert.IsType<MySqlConnection>(connection);

        var builder = new MySqlConnectionStringBuilder(connection.ConnectionString);

        Assert.Equal("localhost", builder.Server);
        Assert.Equal("testdb", builder.Database);
        Assert.Equal("root", builder.UserID);
        Assert.Equal("1234", builder.Password);
    }
}