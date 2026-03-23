using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using MicroServicoUser.Inf.Repository;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class LoginLastTests
{
    private static User BuildUser(
        int id = 1,
        string? password = null,
        string role = "1",
        bool status = true,
        int firstLogin = 1)
    {
        var plainPassword = password ?? "Abc123!@#";

        return new User
        {
            Id = id,
            Username = "testuser",
            PasswordHash = RegistrationHelpers.HashPassword(plainPassword),
            FirstName = "Juan",
            LastName = "Perez",
            Email = "juan@test.com",
            Role = role,
            Status = status,
            FirstLogin = firstLogin,
            CreatedBy = 1,
            CreatedDate = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task ChangePasswordFirstLogin_ShouldFail_WhenNewPasswordIsSameAsCurrent()
    {
        var user = BuildUser(password: "Init123!@", firstLogin: 1);
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetById(user.Id))
            .ReturnsAsync(Result<User?>.Success(user));

        var service = new Login(repoMock.Object);

        var result = await service.ChangePasswordFirstLogin(user.Id, "Init123!@", "Init123!@");

        Assert.False(result.ok);
        Assert.Equal("La nueva contraseña debe ser diferente a la actual.", result.error);
    }

    [Fact]
    public async Task ChangePasswordFirstLogin_ShouldFail_WhenUpdateFails()
    {
        var user = BuildUser(password: "Init123!@", firstLogin: 1);
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetById(user.Id))
            .ReturnsAsync(Result<User?>.Success(user));

        repoMock.Setup(r => r.Update(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Failure("db error"));

        var service = new Login(repoMock.Object);

        var result = await service.ChangePasswordFirstLogin(user.Id, "Init123!@", "Next123!@#");

        Assert.False(result.ok);
        Assert.Contains("db error", result.error);
    }

    [Fact]
    public async Task ValidateLogin_ShouldFail_WhenUserIsInactive()
    {
        var user = BuildUser(password: "Correct123!@", status: false);
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(user.Username!))
            .ReturnsAsync(Result<User?>.Success(user));

        var service = new Login(repoMock.Object);

        var result = await service.ValidateLogin(user.Username!, "Correct123!@");

        Assert.False(result.ok);
    }
}