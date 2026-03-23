using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using MicroServicoUser.Inf.Repository;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class RegistrationExtraTests
{
    [Fact]
    public async Task RegisterUser_ShouldUseRoleZero_WhenRoleIsUnknown()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("Juan", "Perez", "juan@test.com", "UnknownRole", 1);

        Assert.True(result.ok);

        repoMock.Verify(r => r.Insert(It.Is<User>(u => u.Role == "0")), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_ShouldFallbackToUser_WhenNamesAndEmailDoNotProduceLetters()
    {
        var repoMock = new Mock<IRepository>();

        repoMock.Setup(r => r.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(Result<User?>.Success(null));

        repoMock.Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var service = new Registration(repoMock.Object);

        var result = await service.RegisterUser("123", "456", "!!!@test.com", "Admin", 1);

        Assert.True(result.ok);
        Assert.Equal("user", result.generatedUsername);
    }
}