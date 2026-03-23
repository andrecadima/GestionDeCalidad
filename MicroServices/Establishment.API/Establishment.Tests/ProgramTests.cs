using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Establishment.Tests;

public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProgramTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "12345678901234567890123456789012",
                    ["Jwt:Issuer"] = "test-issuer",
                    ["Jwt:Audience"] = "test-audience",
                    ["ConnectionStrings:EstablishmentDB"] = "Server=localhost;Database=testdb;Uid=root;Pwd=1234;"
                };

                config.AddInMemoryCollection(settings);
            });
        });
    }

    [Fact]
    public async Task Application_Should_Start()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/");

        Assert.NotNull(response);
    }

    [Fact]
    public async Task Protected_Endpoint_Should_Return_Unauthorized_When_No_Token()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/Establishment");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}