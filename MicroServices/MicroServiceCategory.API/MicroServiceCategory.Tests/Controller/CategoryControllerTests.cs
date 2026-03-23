using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MicroServiceCategory.API.Controllers;
using MicroServiceCategory.Application.Services;
using MicroServiceCategory.Domain.Entities;
using MicroServiceCategory.Domain.Interfaces;

namespace MicroServiceCategory.Tests.Controllers;

public class CategoryControllerTests
{
    private readonly Mock<IRepository<Category>> _repositoryMock;
    private readonly CategoryService _service;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _repositoryMock = new Mock<IRepository<Category>>();
        _service = new CategoryService(_repositoryMock.Object);
        _controller = new CategoryController(_service);
    }

    private static Category CreateValidCategory()
    {
        return new Category
        {
            Id = 1,
            Name = "Bebidas",
            Description = "Categoria general",
            BaseAmount = 100,
            CreatedBy = 1
        };
    }

    [Fact]
    public async Task Select_Should_Return_Ok_When_Success()
    {
        var list = new List<Category> { CreateValidCategory() };

        _repositoryMock.Setup(r => r.Select()).ReturnsAsync(list);

        var result = await _controller.Select();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Select_Should_Return_500_When_Server_Error()
    {
        _repositoryMock.Setup(r => r.Select()).ThrowsAsync(new System.Exception("DB error"));

        var result = await _controller.Select();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Search_Should_Return_Ok_When_Success()
    {
        var list = new List<Category> { CreateValidCategory() };

        _repositoryMock.Setup(r => r.Search("Beb")).ReturnsAsync(list);

        var result = await _controller.Search("Beb");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Search_Should_Return_BadRequest_When_Invalid_Input()
    {
        var result = await _controller.Search("");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Search_Should_Return_500_When_Server_Error()
    {
        _repositoryMock.Setup(r => r.Search("Beb")).ThrowsAsync(new System.Exception("DB error"));

        var result = await _controller.Search("Beb");

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Create_Should_Return_BadRequest_When_ModelState_Is_Invalid()
    {
        _controller.ModelState.AddModelError("Name", "El nombre es obligatorio");
        var category = CreateValidCategory();

        var result = await _controller.Create(category);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_Should_Return_CreatedAtAction_When_Success()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Insert(category)).ReturnsAsync(10);

        var result = await _controller.Create(category);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("Select", created.ActionName);
    }

    [Fact]
    public async Task Create_Should_Return_500_When_Service_Fails()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Insert(category)).ThrowsAsync(new System.Exception("DB error"));

        var result = await _controller.Create(category);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Update_Should_Return_BadRequest_When_ModelState_Is_Invalid()
    {
        _controller.ModelState.AddModelError("Name", "El nombre es obligatorio");
        var category = CreateValidCategory();

        var result = await _controller.Update(1, category);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_Should_Return_Ok_When_Success()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Update(It.IsAny<Category>())).ReturnsAsync(1);

        var result = await _controller.Update(1, category);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1, category.Id);
    }

    [Fact]
    public async Task Update_Should_Return_NotFound_When_Service_Returns_NotFound()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Update(It.IsAny<Category>())).ReturnsAsync(0);

        var result = await _controller.Update(1, category);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Should_Return_Ok_When_Success()
    {
        _repositoryMock.Setup(r => r.Delete(It.IsAny<Category>())).ReturnsAsync(1);

        var result = await _controller.Delete(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Should_Return_NotFound_When_Service_Returns_NotFound()
    {
        _repositoryMock.Setup(r => r.Delete(It.IsAny<Category>())).ReturnsAsync(0);

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Get_Should_Return_Ok_When_Success()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.SelectById(1)).ReturnsAsync(category);

        var result = await _controller.Get(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Get_Should_Return_NotFound_When_Category_Does_Not_Exist()
    {
        _repositoryMock.Setup(r => r.SelectById(1)).ReturnsAsync((Category)null!);

        var result = await _controller.Get(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Get_Should_Return_500_When_Service_Throws_Exception()
    {
        _repositoryMock.Setup(r => r.SelectById(1)).ThrowsAsync(new System.Exception("DB error"));

        var result = await _controller.Get(1);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}