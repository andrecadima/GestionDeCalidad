using MicroServicoUser.Inf.Persistence.Database;
using MicroServicoUser.Inf.Repository;
using Microsoft.Extensions.Configuration;
using MicroServicioUser.Dom.Entities;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class UserRepositoryFailureTests
{
    private static MySqlConnectionManager BuildInvalidManager()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:MysqlUserServicioDB"] =
                    "Server=127.0.0.1;Port=1;Database=test;Uid=test;Pwd=test;Connection Timeout=1;"
            })
            .Build();

        return new MySqlConnectionManager(config);
    }

    [Fact]
    public async Task GetAll_ShouldReturnFailure_WhenConnectionFails()
    {
        var repo = new UserRepository(BuildInvalidManager());

        var result = await repo.GetAll();

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task GetById_ShouldReturnFailure_WhenConnectionFails()
    {
        var repo = new UserRepository(BuildInvalidManager());

        var result = await repo.GetById(1);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task GetByUsername_ShouldReturnFailure_WhenConnectionFails()
    {
        var repo = new UserRepository(BuildInvalidManager());

        var result = await repo.GetByUsername("juan");

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Search_ShouldReturnFailure_WhenConnectionFails()
    {
        var repo = new UserRepository(BuildInvalidManager());

        var result = await repo.Search("juan");

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Insert_ShouldReturnFailure_WhenConnectionFails()
    {
        var repo = new UserRepository(BuildInvalidManager());

        var result = await repo.Insert(new User
        {
            Username = "juan",
            PasswordHash = "hash",
            FirstName = "Juan",
            LastName = "Perez",
            Email = "juan@test.com",
            Role = "1",
            CreatedBy = 1,
            Status = true,
            FirstLogin = 1
        });

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Update_ShouldReturnFailure_WhenConnectionFails()
    {
        var repo = new UserRepository(BuildInvalidManager());

        var result = await repo.Update(new User
        {
            Id = 1,
            Username = "juan",
            PasswordHash = "hash",
            FirstName = "Juan",
            LastName = "Perez",
            Email = "juan@test.com",
            Role = "1",
            CreatedBy = 1,
            Status = true,
            FirstLogin = 1
        });

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Delete_ShouldReturnFailure_WhenConnectionFails()
    {
        var repo = new UserRepository(BuildInvalidManager());

        var result = await repo.Delete(1);

        Assert.True(result.IsFailure);
    }
}