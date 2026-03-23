using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using MicroServicoUser.Inf.Repository;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class RegistrationTests
{
    private static User? ExistingUser(string username = "j_perez")
    {
        return new User
        {
            Id = 99,
            Username = username,
            PasswordHash = RegistrationHelpers.HashPassword("Abc123!@#"),
            FirstName = "Juan",
            LastName = "Perez",
            Email = "juan@test.com",
            Role = "1",
            Status = true,
            FirstLogin = 1,
            CreatedBy = 1,
            CreatedDate = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task GeneratePassword_ShouldReturnPasswordWithRequestedLength()
    {
        var repoMock = new Mock<IRepository>();
        var service = new Registration(repoMock.Object);

        var password = await service.GeneratePassword(10);

        Assert.Equal(10, password.Length);
    }

    [Fact]
    public async Task RegisterUser_ShouldFail_WhenRequiredDataIsMissing()
    {
        var repoMock = new Mock<IRepository>();
        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("", "Perez", "juan@test.com", "Admin", 1);

        Assert.False(result.ok);
        Assert.Equal("Faltan datos obligatorios.", result.error);
    }

    [Fact]
    public async Task RegisterUser_ShouldCreateUsername_FromFirstAndLastName()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("Juan", "Perez", "juan@test.com", "Admin", 1);

        Assert.True(result.ok);
        Assert.Equal("j_perez", result.generatedUsername);
        Assert.NotNull(result.generatedPassword);
        Assert.Equal(10, result.generatedPassword!.Length);
    }

    [Fact]
    public async Task RegisterUser_ShouldAddSuffix_WhenUsernameAlreadyExists()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.SetupSequence(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(ExistingUser("j_perez")))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("Juan", "Perez", "juan@test.com", "Admin", 1);

        Assert.True(result.ok);
        Assert.Equal("j_perez2", result.generatedUsername);
    }

    [Fact]
    public async Task RegisterUser_ShouldUseEmailLocalPart_WhenNamesDoNotGenerateUsername()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("123", "456", "mail.user@test.com", "Contador", 1);

        Assert.True(result.ok);
        Assert.Equal("mailuser", result.generatedUsername);
    }

    [Fact]
    public async Task RegisterUser_ShouldInsertHashedPassword_AndMappedRole()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("Juan", "Perez", "juan@test.com", "Admin", 7);

        Assert.True(result.ok);

        repoMock.Verify(r => r.Insert(It.Is<User>(u =>
            u.Username == "j_perez" &&
            u.Email == "juan@test.com" &&
            u.Role == "1" &&
            u.CreatedBy == 7 &&
            u.FirstLogin == 1 &&
            u.Status == true &&
            RegistrationHelpers.VerifyPassword(result.generatedPassword!, u.PasswordHash)
        )), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnError_WhenInsertFails()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Failure("Error al insertar"));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("Juan", "Perez", "juan@test.com", "Admin", 1);

        Assert.False(result.ok);
        Assert.Equal("Error al insertar", result.error);
    }
}