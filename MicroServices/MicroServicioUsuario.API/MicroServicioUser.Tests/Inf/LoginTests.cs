using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using MicroServicoUser.Inf.Repository;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class LoginTests
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
    public async Task ChangePassword_ShouldFail_WhenUserNotFound()
    {
        var repoMock = new Mock<IRepository>();
        repoMock.Setup(r => r.GetById(1))
            .ReturnsAsync(Result<User?>.Failure("Usuario no encontrado."));

        var service = new Login(repoMock.Object);

        var result = await service.ChangePassword(1, "Abc123!@#", "New123!@#");

        Assert.False(result.ok);
        Assert.Equal("Usuario no encontrado.", result.error);
    }

    [Fact]
    public async Task ChangePassword_ShouldFail_WhenCurrentPasswordIsIncorrect()
    {
        var user = BuildUser(password: "Correct123!@");
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetById(user.Id))
            .ReturnsAsync(Result<User?>.Success(user));

        var service = new Login(repoMock.Object);

        var result = await service.ChangePassword(user.Id, "Wrong123!@", "New123!@#");

        Assert.False(result.ok);
        Assert.Equal("La contraseña actual no es correcta.", result.error);
    }

    [Fact]
    public async Task ChangePassword_ShouldFail_WhenNewPasswordIsSameAsCurrent()
    {
        var user = BuildUser(password: "Same123!@");
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetById(user.Id))
            .ReturnsAsync(Result<User?>.Success(user));

        var service = new Login(repoMock.Object);

        var result = await service.ChangePassword(user.Id, "Same123!@", "Same123!@");

        Assert.False(result.ok);
        Assert.Equal("La nueva contraseña debe ser diferente a la actual.", result.error);
    }

    [Fact]
    public async Task ChangePassword_ShouldSucceed_WhenDataIsValid()
    {
        var user = BuildUser(password: "Old123!@");
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetById(user.Id))
            .ReturnsAsync(Result<User?>.Success(user));

        repoMock.Setup(r => r.Update(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Login(repoMock.Object);

        var result = await service.ChangePassword(user.Id, "Old123!@", "New123!@#");

        Assert.True(result.ok);
        Assert.Null(result.error);

        repoMock.Verify(r => r.Update(It.Is<User>(u =>
            u.Id == user.Id &&
            u.PasswordHash != null &&
            RegistrationHelpers.VerifyPassword("New123!@#", u.PasswordHash)
        )), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordFirstLogin_ShouldFail_WhenUserAlreadyChangedPassword()
    {
        var user = BuildUser(password: "Init123!@", firstLogin: 0);
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetById(user.Id))
            .ReturnsAsync(Result<User?>.Success(user));

        var service = new Login(repoMock.Object);

        var result = await service.ChangePasswordFirstLogin(user.Id, "Init123!@", "New123!@#");

        Assert.False(result.ok);
        Assert.Equal("Este usuario ya ha cambiado su contraseña inicial.", result.error);
    }

    [Fact]
    public async Task ChangePasswordFirstLogin_ShouldSucceed_WhenValid()
    {
        var user = BuildUser(password: "Init123!@", firstLogin: 1);
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetById(user.Id))
            .ReturnsAsync(Result<User?>.Success(user));

        repoMock.Setup(r => r.Update(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Login(repoMock.Object);

        var result = await service.ChangePasswordFirstLogin(user.Id, "Init123!@", "Next123!@#");

        Assert.True(result.ok);
        Assert.Null(result.error);

        repoMock.Verify(r => r.Update(It.Is<User>(u =>
            u.FirstLogin == 0 &&
            RegistrationHelpers.VerifyPassword("Next123!@#", u.PasswordHash)
        )), Times.Once);
    }

    [Fact]
    public async Task ValidateLogin_ShouldFail_WhenRepositoryFails()
    {
        var repoMock = new Mock<IRepository>();
        repoMock.Setup(r => r.GetByUsername("testuser"))
            .ReturnsAsync(Result<User?>.Failure("No existe"));

        var service = new Login(repoMock.Object);

        var result = await service.ValidateLogin("testuser", "Abc123!@#");

        Assert.False(result.ok);
        Assert.Equal("No existe", result.error);
    }

    [Fact]
    public async Task ValidateLogin_ShouldFail_WhenPasswordIsIncorrect()
    {
        var user = BuildUser(password: "Correct123!@", role: "1", status: true, firstLogin: 1);
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(user.Username))
            .ReturnsAsync(Result<User?>.Success(user));

        var service = new Login(repoMock.Object);

        var result = await service.ValidateLogin(user.Username, "Wrong123!@");

        Assert.False(result.ok);
        Assert.Equal("Contraseña incorrecta.", result.error);
    }

    [Fact]
    public async Task ValidateLogin_ShouldReturnMappedRole_WhenSuccessful()
    {
        var user = BuildUser(password: "Correct123!@", role: "1", status: true, firstLogin: 0);
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(user.Username))
            .ReturnsAsync(Result<User?>.Success(user));

        var service = new Login(repoMock.Object);

        var result = await service.ValidateLogin(user.Username, "Correct123!@");

        Assert.True(result.ok);
        Assert.Equal(user.Id, result.userId);
        Assert.Equal("Admin", result.role);
        Assert.True(result.isFirstLogin);
        Assert.Null(result.error);
    }
}