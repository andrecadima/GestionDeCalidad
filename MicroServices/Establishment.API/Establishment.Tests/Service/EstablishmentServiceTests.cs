using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Establishment.App.Service;
using Establishment.Dom.Interface;
using Establishment.Dom.Model;

namespace Establishment.Tests.Service;

public class EstablishmentServiceTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly EstablishmentService _service;

    public EstablishmentServiceTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _service = new EstablishmentService(_repositoryMock.Object);
    }

    [Fact]
    public async Task Insert_Should_Return_Result_From_Repository()
    {
        var entity = new Establishment.Dom.Model.Establishment();
        var expected = Result<int>.Success(1);

        _repositoryMock.Setup(r => r.Insert(entity)).ReturnsAsync(expected);

        var result = await _service.Insert(entity);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task Select_Should_Return_Result_From_Repository()
    {
        var list = new List<Establishment.Dom.Model.Establishment>
        {
            new Establishment.Dom.Model.Establishment()
        };

        var expected = Result<List<Establishment.Dom.Model.Establishment>>.Success(list);

        _repositoryMock.Setup(r => r.Select()).ReturnsAsync(expected);

        var result = await _service.Select();

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
    }

    [Fact]
    public async Task SelectById_Should_Return_Result_From_Repository()
    {
        var entity = new Establishment.Dom.Model.Establishment();
        var expected = Result<Establishment.Dom.Model.Establishment>.Success(entity);

        _repositoryMock.Setup(r => r.SelectById(10)).ReturnsAsync(expected);

        var result = await _service.SelectById(10);

        Assert.True(result.IsSuccess);
        Assert.Same(entity, result.Value);
    }

    [Fact]
    public async Task Update_Should_Return_Result_From_Repository()
    {
        var entity = new Establishment.Dom.Model.Establishment();
        var expected = Result<int>.Success(1);

        _repositoryMock.Setup(r => r.Update(entity)).ReturnsAsync(expected);

        var result = await _service.Update(entity);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task Delete_Should_Return_Result_From_Repository()
    {
        var entity = new Establishment.Dom.Model.Establishment();
        var expected = Result<int>.Success(1);

        _repositoryMock.Setup(r => r.Delete(entity)).ReturnsAsync(expected);

        var result = await _service.Delete(entity);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task Search_Should_Return_Result_From_Repository()
    {
        var list = new List<Establishment.Dom.Model.Establishment>
        {
            new Establishment.Dom.Model.Establishment()
        };

        var expected = Result<IEnumerable<Establishment.Dom.Model.Establishment>>.Success(list);

        _repositoryMock.Setup(r => r.Search("test")).ReturnsAsync(expected);

        var result = await _service.Search("test");

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
    }
}