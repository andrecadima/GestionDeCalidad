using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroServicioUser.App.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MicroServicioUser.Tests.App;

public class JwtServiceTests
{
    private static IConfiguration BuildConfig()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"Jwt:Key", "THIS_IS_A_SUPER_SECRET_KEY_12345"},
            {"Jwt:Issuer", "testIssuer"},
            {"Jwt:Audience", "testAudience"},
            {"Jwt:ExpireMinutes", "60"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
    }

    [Fact]
    public async Task GenerateToken_ShouldReturnValidToken()
    {
        var config = BuildConfig();
        var service = new JwtService(config);

        var result = await service.GenerateToken(1, "Admin");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task GenerateToken_ShouldContainThreeParts()
    {
        var config = BuildConfig();
        var service = new JwtService(config);

        var result = await service.GenerateToken(1, "Admin");

        var parts = result.Value.Split('.');
        Assert.Equal(3, parts.Length);
    }
}