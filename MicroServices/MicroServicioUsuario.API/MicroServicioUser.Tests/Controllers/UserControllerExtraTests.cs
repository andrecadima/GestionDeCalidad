using MicroServicioUsuario.API.Controllers;
using MicroServicioUser.App.Services;
using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Controllers;

public class UserControllerExtraTests
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
    public async Task Login_ShouldReturnOk_WithFailurePayload_WhenCredentialsInvalid()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var loginMock = new Mock<ILogin>();
        var jwtMock = new Mock<IJwtService>();

        loginMock.Setup(x => x.ValidateLogin(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((false, null, null, "Contraseña incorrecta.", false));

        var controller = BuildController(repo, loginMock, jwtMock: jwtMock);

        var result = await controller.Login(new LoginDto
        {
            Username = "juan",
            Password = "bad"
        });

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenRegistrationSucceeds()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var registrationMock = new Mock<IRegistration>();

        registrationMock.Setup(x => x.RegisterUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .ReturnsAsync((true, "user", "pass", null));

        var controller = BuildController(repo, registrationMock: registrationMock);

        var result = await controller.Register(new RegisterDto
        {
            FirstName = "Juan",
            LastName = "Perez",
            Email = "juan@test.com",
            Role = "Admin",
            CreatedBy = 1
        });

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenModelStateInvalid()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var controller = BuildController(repo);

        controller.ModelState.AddModelError("x", "invalid");

        var result = await controller.Update(new User());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenModelStateInvalid()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var controller = BuildController(repo);

        controller.ModelState.AddModelError("x", "invalid");

        var result = await controller.Create(new User());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturn500_WhenGenericFailure()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.Delete(1))
            .ReturnsAsync(Result<int>.Failure("error genérico"));

        var controller = BuildController(repo);

        var result = await controller.Delete(1);

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, obj.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturn500_WhenGenericFailure()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.Update(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Failure("error genérico"));

        var controller = BuildController(repo);

        var result = await controller.Update(new User { Id = 1 });

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, obj.StatusCode);
    }
}