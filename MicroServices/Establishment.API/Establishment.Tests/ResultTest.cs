using Xunit;
using Establishment.Dom.Model;

namespace Establishment.Tests;

public class ResultTests
{
    [Fact]
    public void Success_Should_Create_Result_With_IsSuccess_True()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Errors);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Failure_Should_Create_Result_With_IsFailure_True()
    {
        // Act
        var result = Result.Failure("Error de prueba");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Contains("Error de prueba", result.Errors);
    }

    [Fact]
    public void Failure_Should_Store_Multiple_Errors()
    {
        // Act
        var result = Result.Failure("Error 1", "Error 2");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains("Error 1", result.Errors);
        Assert.Contains("Error 2", result.Errors);
    }

    [Fact]
    public void Generic_Success_Should_Create_Result_With_Value()
    {
        // Act
        var result = Result<int>.Success(10);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(10, result.Value);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Generic_Failure_Should_Create_Result_With_Default_Value_And_Error()
    {
        // Act
        var result = Result<string>.Failure("Fallo genérico");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.Single(result.Errors);
        Assert.Contains("Fallo genérico", result.Errors);
    }
}