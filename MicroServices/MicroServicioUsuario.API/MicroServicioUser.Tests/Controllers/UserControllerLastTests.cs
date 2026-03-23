using MicroServicioUsuario.API.Controllers;
using MicroServicioUser.App.Services;
using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Controllers;

public class UserControllerLastTests
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
    public async Task GetById_ShouldReturnOk_WithNullValue_WhenSuccessAndNull()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.GetById(1))
            .ReturnsAsync(Result<User?>.Success(null));

        var controller = BuildController(repo);

        var result = await controller.GetById(1);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenRegistrationFails()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var registrationMock = new Mock<IRegistration>();

        registrationMock.Setup(x => x.RegisterUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .ReturnsAsync((false, null, null, "error"));

        var controller = BuildController(repo, registrationMock: registrationMock);

        var result = await controller.Register(new RegisterDto
        {
            FirstName = "Juan",
            LastName = "Perez",
            Email = "juan@test.com",
            Role = "Admin",
            CreatedBy = 1
        });

        Assert.IsType<OkObjectResult>(result.Result);
    }
}