using Xunit;
using MicroServiceCategory.Domain.Patterns;

namespace MicroServiceCategory.Tests.Patterns;

public class ResultTests
{
    [Fact]
    public void Success_Should_Create_Result_With_IsSuccess_True()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Errors);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Failure_Should_Create_Result_With_Error()
    {
        var result = Result.Failure("Error de prueba");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Contains("Error de prueba", result.Errors);
    }

    [Fact]
    public void Generic_Success_Should_Store_Value()
    {
        var result = Result<int>.Success(10);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(10, result.Value);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Generic_Failure_Should_Store_Default_Value_And_Errors()
    {
        var result = Result<string>.Failure("Fallo genérico");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.Single(result.Errors);
        Assert.Contains("Fallo genérico", result.Errors);
    }

    [Fact]
    public void Generic_Failure_With_Int_Should_Return_Default_Zero()
    {
        var result = Result<int>.Failure("Error");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(0, result.Value);
        Assert.Single(result.Errors);
    }
}