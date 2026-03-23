using Microsoft.Extensions.Configuration;
using Xunit;
using Establishment.Inf.Persistence;
using MySql.Data.MySqlClient;

namespace Establishment.Tests.Persistence;

public class MySqlConnectionDBTests
{
	[Fact]
	public void GetConnection_Should_Return_MySqlConnection_With_Configured_ConnectionString()
	{
		var settings = new Dictionary<string, string?>
		{
			["ConnectionStrings:EstablishmentDB"] = "Server=localhost;Database=testdb;Uid=root;Pwd=1234;"
		};

		IConfiguration configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(settings)
			.Build();

		var db = new MySqlConnectionDB(configuration);

		var connection = db.GetConnection();

		Assert.NotNull(connection);
		Assert.IsType<MySqlConnection>(connection);
		Assert.Equal("testdb", connection.Database);
	}
}