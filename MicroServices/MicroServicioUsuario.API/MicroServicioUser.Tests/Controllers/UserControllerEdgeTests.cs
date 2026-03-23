using MicroServicioUsuario.API.Controllers;
using MicroServicioUser.App.Services;
using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Controllers;

public class UserControllerEdgeTests
{
    private static UserController BuildController(
        Mock<IRepositoryService<User>> repoServiceMock,
        Mock<ILogin>? loginMock = null,
        Mock<IRegistration>? registrationMock = null,
        Mock<IJwtService>? jwtMock = null,
        Mock<IEmailService>? emailAdapterMock = null)
    {
        loginMock ??= new Mock<ILogin>();
        registrationMock ??= new Mock<IRegistration>();
        jwtMock ??= new Mock<IJwtService>();
        emailAdapterMock ??= new Mock<IEmailService>();

        var loginService = new LoginService(loginMock.Object);
        var emailService = new EmailService(emailAdapterMock.Object);
        var registrationService = new RegistrationService(registrationMock.Object, emailService);

        return new UserController(
            repoServiceMock.Object,
            loginService,
            registrationService,
            jwtMock.Object
        );
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenServiceReturnsOkButNullRoleOrUserId()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var loginMock = new Mock<ILogin>();
        var jwtMock = new Mock<IJwtService>();

        loginMock.Setup(x => x.ValidateLogin(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((true, null, null, "incomplete", false));

        var controller = BuildController(repo, loginMock, jwtMock: jwtMock);

        var result = await controller.Login(new LoginDto
        {
            Username = "juan",
            Password = "123"
        });

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task ChangePasswordFirstLogin_ShouldReturnOk_WhenServiceFails()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var loginMock = new Mock<ILogin>();

        loginMock.Setup(x => x.ChangePasswordFirstLogin(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((false, "error"));

        var controller = BuildController(repo, loginMock);

        var result = await controller.ChangePasswordFirstLogin(new ChangePasswordDto
        {
            Id = 1,
            CurrentPassword = "Old123!@",
            NewPassword = "New123!@#"
        });

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(ok.Value);
    }
}