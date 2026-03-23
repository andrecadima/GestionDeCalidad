using MicroServicioUser.App.Services;
using MicroServicioUser.Dom.Interfaces;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.App;

public class RegistrationServiceTests
{
    [Fact]
    public async Task RegisterUser_ShouldReturnError_WhenRegistrationFails()
    {
        var regMock = new Mock<IRegistration>();
        var emailAdapterMock = new Mock<IEmailService>();
        var emailService = new EmailService(emailAdapterMock.Object);

        regMock.Setup(x => x.RegisterUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .ReturnsAsync((false, null, null, "Error"));

        var service = new RegistrationService(regMock.Object, emailService);

        var result = await service.RegisterUser("Juan", "Perez", "test@test.com", "Admin", 1);

        Assert.False(result.ok);
        Assert.Equal("Error", result.error);
    }

    [Fact]
    public async Task RegisterUser_ShouldSucceed_WhenValid()
    {
        var regMock = new Mock<IRegistration>();
        var emailAdapterMock = new Mock<IEmailService>();
        var emailService = new EmailService(emailAdapterMock.Object);

        regMock.Setup(x => x.RegisterUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .ReturnsAsync((true, "user", "pass", null));

        var service = new RegistrationService(regMock.Object, emailService);

        var result = await service.RegisterUser("Juan", "Perez", "test@test.com", "Admin", 1);

        Assert.True(result.ok);
        Assert.Equal("user", result.generatedUsername);
        Assert.Equal("pass", result.generatedPassword);
        Assert.Null(result.error);
    }
}