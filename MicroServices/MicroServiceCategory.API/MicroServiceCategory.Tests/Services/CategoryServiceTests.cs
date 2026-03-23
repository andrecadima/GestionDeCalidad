using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MicroServiceCategory.Application.Services;
using MicroServiceCategory.Domain.Entities;
using MicroServiceCategory.Domain.Interfaces;

namespace MicroServiceCategory.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<IRepository<Category>> _repositoryMock;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Category>>();
        _service = new CategoryService(_repositoryMock.Object);
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
    public async Task Insert_Should_Return_Success_When_Repository_Inserts_Correctly()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Insert(category)).ReturnsAsync(5);

        var result = await _service.Insert(category);

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value);
        Assert.Equal((byte)1, category.Status);
        Assert.NotEqual(default, category.CreatedDate);
        Assert.NotEqual(default, category.LastUpdate);
    }

    [Fact]
    public async Task Insert_Should_Return_ServerError_When_Exception_Is_Thrown()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Insert(category)).ThrowsAsync(new Exception("DB error"));

        var result = await _service.Insert(category);

        Assert.False(result.IsSuccess);
        Assert.Contains("ServerError", result.Errors);
        Assert.Contains("DB error", result.Errors);
    }

    [Fact]
    public async Task Update_Should_Return_Success_When_Repository_Returns_Affected_Rows()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Update(category)).ReturnsAsync(1);

        var result = await _service.Update(category);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
        Assert.NotEqual(default, category.LastUpdate);
    }

    [Fact]
    public async Task Update_Should_Return_NotFound_When_Repository_Returns_Zero()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Update(category)).ReturnsAsync(0);

        var result = await _service.Update(category);

        Assert.False(result.IsSuccess);
        Assert.Contains("NotFound", result.Errors);
    }

    [Fact]
    public async Task Update_Should_Return_ServerError_When_Exception_Is_Thrown()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Update(category)).ThrowsAsync(new Exception("DB error"));

        var result = await _service.Update(category);

        Assert.False(result.IsSuccess);
        Assert.Contains("ServerError", result.Errors);
    }

    [Fact]
    public async Task Delete_Should_Return_Success_When_Repository_Returns_Affected_Rows()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Delete(category)).ReturnsAsync(1);

        var result = await _service.Delete(category);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task Delete_Should_Return_NotFound_When_Repository_Returns_Zero()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Delete(category)).ReturnsAsync(0);

        var result = await _service.Delete(category);

        Assert.False(result.IsSuccess);
        Assert.Contains("NotFound", result.Errors);
    }

    [Fact]
    public async Task Delete_Should_Return_ServerError_When_Exception_Is_Thrown()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.Delete(category)).ThrowsAsync(new Exception("DB error"));

        var result = await _service.Delete(category);

        Assert.False(result.IsSuccess);
        Assert.Contains("ServerError", result.Errors);
    }

    [Fact]
    public async Task Select_Should_Return_Success_When_Repository_Returns_List()
    {
        var list = new List<Category> { CreateValidCategory() };

        _repositoryMock.Setup(r => r.Select()).ReturnsAsync(list);

        var result = await _service.Select();

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
    }

    [Fact]
    public async Task Select_Should_Return_ServerError_When_Exception_Is_Thrown()
    {
        _repositoryMock.Setup(r => r.Select()).ThrowsAsync(new Exception("DB error"));

        var result = await _service.Select();

        Assert.False(result.IsSuccess);
        Assert.Contains("ServerError", result.Errors);
    }

    [Fact]
    public async Task Search_Should_Return_InvalidInput_When_Property_Is_Empty()
    {
        var result = await _service.Search("");

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.StartsWith("InvalidInput"));
    }

    [Fact]
    public async Task Search_Should_Return_Success_When_Repository_Returns_List()
    {
        var list = new List<Category> { CreateValidCategory() };

        _repositoryMock.Setup(r => r.Search("Beb")).ReturnsAsync(list);

        var result = await _service.Search("Beb");

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
    }

    [Fact]
    public async Task Search_Should_Return_ServerError_When_Exception_Is_Thrown()
    {
        _repositoryMock.Setup(r => r.Search("Beb")).ThrowsAsync(new Exception("DB error"));

        var result = await _service.Search("Beb");

        Assert.False(result.IsSuccess);
        Assert.Contains("ServerError", result.Errors);
    }

    [Fact]
    public async Task SelectById_Should_Return_Success_When_Category_Exists()
    {
        var category = CreateValidCategory();

        _repositoryMock.Setup(r => r.SelectById(1)).ReturnsAsync(category);

        var result = await _service.SelectById(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Id);
    }

    [Fact]
    public async Task SelectById_Should_Return_NotFound_When_Category_Is_Null()
    {
        _repositoryMock.Setup(r => r.SelectById(1)).ReturnsAsync((Category)null!);

        var result = await _service.SelectById(1);

        Assert.False(result.IsSuccess);
        Assert.Contains("NotFound", result.Errors);
    }

    [Fact]
    public async Task SelectById_Should_Return_ServerError_When_Exception_Is_Thrown()
    {
        _repositoryMock.Setup(r => r.SelectById(1)).ThrowsAsync(new Exception("DB error"));

        var result = await _service.SelectById(1);

        Assert.False(result.IsSuccess);
        Assert.Contains("ServerError", result.Errors);
    }
}