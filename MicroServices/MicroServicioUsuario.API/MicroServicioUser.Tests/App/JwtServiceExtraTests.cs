using MicroServicioUser.App.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MicroServicioUser.Tests.App;

public class JwtServiceExtraTests
{
    [Fact]
    public async Task GenerateToken_ShouldThrow_WhenKeyMissing()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "issuer",
                ["Jwt:Audience"] = "audience",
                ["Jwt:ExpireMinutes"] = "60"
            })
            .Build();

        var service = new JwtService(config);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GenerateToken(1, "Admin"));
    }

    [Fact]
    public async Task GenerateToken_ShouldThrow_WhenIssuerMissing()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "THIS_IS_A_SUPER_SECRET_KEY_12345",
                ["Jwt:Audience"] = "audience",
                ["Jwt:ExpireMinutes"] = "60"
            })
            .Build();

        var service = new JwtService(config);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GenerateToken(1, "Admin"));
    }

    [Fact]
    public async Task GenerateToken_ShouldThrow_WhenAudienceMissing()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "THIS_IS_A_SUPER_SECRET_KEY_12345",
                ["Jwt:Issuer"] = "issuer",
                ["Jwt:ExpireMinutes"] = "60"
            })
            .Build();

        var service = new JwtService(config);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GenerateToken(1, "Admin"));
    }

    [Fact]
    public async Task GenerateToken_ShouldThrow_WhenExpireMinutesMissing()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "THIS_IS_A_SUPER_SECRET_KEY_12345",
                ["Jwt:Issuer"] = "issuer",
                ["Jwt:Audience"] = "audience"
            })
            .Build();

        var service = new JwtService(config);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GenerateToken(1, "Admin"));
    }
}