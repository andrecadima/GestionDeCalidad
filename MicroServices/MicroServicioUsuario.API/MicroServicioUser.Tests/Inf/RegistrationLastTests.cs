using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using MicroServicoUser.Inf.Repository;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class RegistrationLastTests
{
    [Fact]
    public async Task RegisterUser_ShouldFail_WhenLastNameIsEmpty()
    {
        var repoMock = new Mock<IRepository>();
        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("Juan", "", "mail.user@test.com", "Admin", 1);

        Assert.False(result.ok);
        Assert.Equal("Faltan datos obligatorios.", result.error);
    }

    [Fact]
    public async Task RegisterUser_ShouldGenerateUsernameWithSuffixThree_WhenTwoUsersAlreadyExist()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.SetupSequence(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(new User { Username = "j_perez" }))
            .ReturnsAsync(Result<User?>.Success(new User { Username = "j_perez2" }))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("Juan", "Perez", "juan@test.com", "Admin", 1);

        Assert.True(result.ok);
        Assert.Equal("j_perez3", result.generatedUsername);
    }

    [Fact]
    public async Task RegisterUser_ShouldFallbackToEmailLocalPart_WhenNamesDoNotProduceLetters()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("123", "456", "mail.user@test.com", "Admin", 1);

        Assert.True(result.ok);
        Assert.Equal("mailuser", result.generatedUsername);
    }
}