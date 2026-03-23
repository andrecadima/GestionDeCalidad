using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroServicioUser.Dom.Rules;
using Xunit;

namespace MicroServicioUser.Tests.Dom;

public class PasswordValidatorTests
{
    [Fact]
    public void Validate_ShouldFail_WhenPasswordIsEmpty()
    {
        var result = PasswordValidator.Validate("");

        Assert.False(result.ok);
        Assert.NotNull(result.error);
    }

    [Fact]
    public void Validate_ShouldFail_WhenPasswordMissingUppercase()
    {
        var result = PasswordValidator.Validate("abc123!@#");

        Assert.False(result.ok);
    }

    [Fact]
    public void Validate_ShouldFail_WhenPasswordMissingNumber()
    {
        var result = PasswordValidator.Validate("Abcdef!@#");

        Assert.False(result.ok);
    }

    [Fact]
    public void Validate_ShouldPass_WhenPasswordIsStrong()
    {
        var result = PasswordValidator.Validate("Abc123!@#");

        Assert.True(result.ok);
        Assert.Null(result.error);
    }

    [Fact]
    public void Validate_ShouldHandleTimeout()
    {
        // string muy grande para intentar forzar timeout
        var longPassword = new string('A', 10000);

        var result = PasswordValidator.Validate(longPassword);

        Assert.False(result.ok);
    }
}
