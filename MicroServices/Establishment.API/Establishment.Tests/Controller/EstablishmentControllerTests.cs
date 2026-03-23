using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Establishment.API.Controller;
using Establishment.App.Service;
using Establishment.Dom.Interface;
using Establishment.Dom.Model;

namespace Establishment.Tests.Controller;

public class EstablishmentControllerTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly EstablishmentService _service;
    private readonly EstablishmentController _controller;

    public EstablishmentControllerTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _service = new EstablishmentService(_repositoryMock.Object);
        _controller = new EstablishmentController(_service);
    }

    [Fact]
    public async Task Insert_Should_Return_CreatedAtAction_When_Success()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.Insert(entity))
            .ReturnsAsync(Result<int>.Success(5));

        var result = await _controller.Insert(entity);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("Get", created.ActionName);
    }

    [Fact]
    public async Task Insert_Should_Return_BadRequest_When_Invalid_Input()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.Insert(entity))
            .ReturnsAsync(Result<int>.Failure("InvalidInput: dato incorrecto"));

        var result = await _controller.Insert(entity);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Select_Should_Return_Ok_When_Success()
    {
        var list = new List<Establishment.Dom.Model.Establishment>
        {
            new Establishment.Dom.Model.Establishment()
        };

        _repositoryMock.Setup(r => r.Select())
            .ReturnsAsync(Result<List<Establishment.Dom.Model.Establishment>>.Success(list));

        var result = await _controller.Select();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Select_Should_Return_NotFound_When_NoRowsAffected()
    {
        _repositoryMock.Setup(r => r.Select())
            .ReturnsAsync(Result<List<Establishment.Dom.Model.Establishment>>.Failure("NoRowsAffected"));

        var result = await _controller.Select();

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Get_Should_Return_Ok_When_Success()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.SelectById(1))
            .ReturnsAsync(Result<Establishment.Dom.Model.Establishment>.Success(entity));

        var result = await _controller.Get(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Get_Should_Return_NotFound_When_NotFound()
    {
        _repositoryMock.Setup(r => r.SelectById(1))
            .ReturnsAsync(Result<Establishment.Dom.Model.Establishment>.Failure("NotFound"));

        var result = await _controller.Get(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_Should_Return_Ok_When_Success()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.Update(entity))
            .ReturnsAsync(Result<int>.Success(1));

        var result = await _controller.Update(entity);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_Should_Return_BadRequest_When_Validation_Error()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.Update(entity))
            .ReturnsAsync(Result<int>.Failure("El campo nombre es obligatorio"));

        var result = await _controller.Update(entity);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Should_Return_Ok_When_Select_And_Delete_Succeed()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.SelectById(2))
            .ReturnsAsync(Result<Establishment.Dom.Model.Establishment>.Success(entity));

        _repositoryMock.Setup(r => r.Delete(entity))
            .ReturnsAsync(Result<int>.Success(1));

        var result = await _controller.Delete(2);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Should_Return_NotFound_When_SelectById_Fails()
    {
        _repositoryMock.Setup(r => r.SelectById(2))
            .ReturnsAsync(Result<Establishment.Dom.Model.Establishment>.Failure("NotFound"));

        var result = await _controller.Delete(2);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Should_Return_ServerError_When_Delete_Fails()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.SelectById(2))
            .ReturnsAsync(Result<Establishment.Dom.Model.Establishment>.Success(entity));

        _repositoryMock.Setup(r => r.Delete(entity))
            .ReturnsAsync(Result<int>.Failure("DatabaseError"));

        var result = await _controller.Delete(2);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Search_Should_Return_Ok_When_Success()
    {
        var list = new List<Establishment.Dom.Model.Establishment>
        {
            new Establishment.Dom.Model.Establishment()
        };

        _repositoryMock.Setup(r => r.Search("abc"))
            .ReturnsAsync(Result<IEnumerable<Establishment.Dom.Model.Establishment>>.Success(list));

        var result = await _controller.Search("abc");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Search_Should_Return_500_When_Failure()
    {
        _repositoryMock.Setup(r => r.Search("abc"))
            .ReturnsAsync(Result<IEnumerable<Establishment.Dom.Model.Establishment>>.Failure("Error interno"));

        var result = await _controller.Search("abc");

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Select_Should_Return_500_When_Unknown_Error()
    {
        _repositoryMock.Setup(r => r.Select())
            .ReturnsAsync(Result<List<Establishment.Dom.Model.Establishment>>.Failure());

        var result = await _controller.Select();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Insert_Should_Return_NotFound_When_NoRowsAffected()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.Insert(entity))
            .ReturnsAsync(Result<int>.Failure("NoRowsAffected"));

        var result = await _controller.Insert(entity);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Insert_Should_Return_500_When_Unknown_Error()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.Insert(entity))
            .ReturnsAsync(Result<int>.Failure("DatabaseError"));

        var result = await _controller.Insert(entity);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Get_Should_Return_BadRequest_When_Validation_Error()
    {
        _repositoryMock.Setup(r => r.SelectById(1))
            .ReturnsAsync(Result<Establishment.Dom.Model.Establishment>.Failure("InvalidInput"));

        var result = await _controller.Get(1);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_Should_Return_NotFound_When_NoRowsAffected()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.Update(entity))
            .ReturnsAsync(Result<int>.Failure("NoRowsAffected"));

        var result = await _controller.Update(entity);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_Should_Return_500_When_Server_Error()
    {
        var entity = new Establishment.Dom.Model.Establishment();

        _repositoryMock.Setup(r => r.Update(entity))
            .ReturnsAsync(Result<int>.Failure("DatabaseError"));

        var result = await _controller.Update(entity);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Delete_Should_Return_BadRequest_When_SelectById_Returns_InvalidInput()
    {
        _repositoryMock.Setup(r => r.SelectById(2))
            .ReturnsAsync(Result<Establishment.Dom.Model.Establishment>.Failure("InvalidInput"));

        var result = await _controller.Delete(2);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}