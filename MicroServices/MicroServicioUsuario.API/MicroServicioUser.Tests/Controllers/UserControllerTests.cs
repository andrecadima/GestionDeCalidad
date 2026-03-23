using MicroServicioUser.App.Services;
using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using MicroServicioUsuario.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MicroServicioUser.Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task Login_ShouldReturnOk_WhenValid()
    {
        var repoServiceMock = new Mock<IRepositoryService<MicroServicioUser.Dom.Entities.User>>();
        var loginMock = new Mock<ILogin>();
        var registrationMock = new Mock<IRegistration>();
        var jwtMock = new Mock<IJwtService>();

        loginMock.Setup(x => x.ValidateLogin(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((true, 1, "Admin", null, true));

        jwtMock.Setup(x => x.GenerateToken(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(Result<string>.Success("fake-token"));

        var loginService = new LoginService(loginMock.Object);
        var registrationService = new RegistrationService(registrationMock.Object, null!);

        var controller = new UserController(
            repoServiceMock.Object,
            loginService,
            registrationService,
            jwtMock.Object
        );

        var result = await controller.Login(new LoginDto
        {
            Username = "test",
            Password = "123"
        });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WithErrorPayload_WhenInvalid()
    {
        var repoServiceMock = new Mock<IRepositoryService<MicroServicioUser.Dom.Entities.User>>();
        var loginMock = new Mock<ILogin>();
        var registrationMock = new Mock<IRegistration>();
        var jwtMock = new Mock<IJwtService>();

        loginMock.Setup(x => x.ValidateLogin(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((false, null, null, "Error", false));

        var loginService = new LoginService(loginMock.Object);
        var registrationService = new RegistrationService(registrationMock.Object, null!);

        var controller = new UserController(
            repoServiceMock.Object,
            loginService,
            registrationService,
            jwtMock.Object
        );

        var result = await controller.Login(new LoginDto
        {
            Username = "test",
            Password = "wrong"
        });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

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
    public async Task ChangePasswordFirstLogin_ShouldReturnBadRequest_WhenModelStateInvalid()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var controller = BuildController(repo);

        controller.ModelState.AddModelError("x", "invalid");

        var result = await controller.ChangePasswordFirstLogin(new ChangePasswordDto());

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task ChangePasswordFirstLogin_ShouldReturnBadRequest_WhenIdMissing()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var controller = BuildController(repo);

        var result = await controller.ChangePasswordFirstLogin(new ChangePasswordDto
        {
            Id = null,
            CurrentPassword = "Old123!@",
            NewPassword = "New123!@#"
        });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenCreatedByMissing()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var controller = BuildController(repo);

        var result = await controller.Register(new RegisterDto
        {
            FirstName = "Juan",
            LastName = "Perez",
            Email = "juan@test.com",
            Role = "Admin",
            CreatedBy = null
        });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenSuccess()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.GetById(1))
            .ReturnsAsync(Result<User?>.Success(new User { Id = 1, Username = "juan" }));

        var controller = BuildController(repo);

        var result = await controller.GetById(1);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ShouldReturn500_WhenFailure()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.GetById(1))
            .ReturnsAsync(Result<User?>.Failure("error"));

        var controller = BuildController(repo);

        var result = await controller.GetById(1);

        var obj = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, obj.StatusCode);
    }

    [Fact]
    public async Task Select_ShouldReturnOk_WhenSuccess()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.GetAll())
            .ReturnsAsync(Result<IEnumerable<User>>.Success(new List<User> { new User { Id = 1 } }));

        var controller = BuildController(repo);

        var result = await controller.Select();

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Select_ShouldReturn500_WhenFailure()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.GetAll())
            .ReturnsAsync(Result<IEnumerable<User>>.Failure("error"));

        var controller = BuildController(repo);

        var result = await controller.Select();

        var obj = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, obj.StatusCode);
    }

    [Fact]
    public async Task Search_ShouldReturnOk_WhenSuccess()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.Search("abc"))
            .ReturnsAsync(Result<IEnumerable<User>>.Success(new List<User> { new User { Id = 1 } }));

        var controller = BuildController(repo);

        var result = await controller.Search("abc");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Search_ShouldReturn500_WhenFailure()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.Search("abc"))
            .ReturnsAsync(Result<IEnumerable<User>>.Failure("error"));

        var controller = BuildController(repo);

        var result = await controller.Search("abc");

        var obj = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, obj.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenSuccess()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.Update(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Success(1));

        var controller = BuildController(repo);

        var result = await controller.Update(new User { Id = 1, Username = "juan" });

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenErrorContainsNoSeEncontro()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.Update(It.IsAny<User>()))
            .ReturnsAsync(Result<int>.Failure("No se encontró"));

        var controller = BuildController(repo);

        var result = await controller.Update(new User { Id = 1, Username = "juan" });

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenSuccess()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.Delete(1))
            .ReturnsAsync(Result<int>.Success(1));

        var controller = BuildController(repo);

        var result = await controller.Delete(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenErrorContainsNoSeEncontro()
    {
        var repo = new Mock<IRepositoryService<User>>();
        repo.Setup(x => x.Delete(1))
            .ReturnsAsync(Result<int>.Failure("No se encontró"));

        var controller = BuildController(repo);

        var result = await controller.Delete(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Login_ShouldFail_WhenUserInactive()
    {
        var repo = new Mock<IRepositoryService<User>>();
        var loginMock = new Mock<ILogin>();
        var jwtMock = new Mock<IJwtService>();

        loginMock.Setup(x => x.ValidateLogin(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((false, null, null, "inactive", false));

        var controller = new UserController(
            repo.Object,
            new LoginService(loginMock.Object),
            new RegistrationService(new Mock<IRegistration>().Object, new EmailService(new Mock<IEmailService>().Object)),
            jwtMock.Object
        );

        var result = await controller.Login(new LoginDto { Username = "a", Password = "b" });

        Assert.IsType<OkObjectResult>(result.Result);
    }
}