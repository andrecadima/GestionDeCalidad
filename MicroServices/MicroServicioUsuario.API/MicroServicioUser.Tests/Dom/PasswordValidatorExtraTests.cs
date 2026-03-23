using MicroServicioUser.Dom.Rules;
using Xunit;

namespace MicroServicioUser.Tests.Dom;

public class PasswordValidatorExtraTests
{
    [Fact]
    public void Validate_ShouldFail_WhenMissingLowercase()
    {
        var result = PasswordValidator.Validate("ABC123!@#");
        Assert.False(result.ok);
    }

    [Fact]
    public void Validate_ShouldFail_WhenMissingSpecialCharacter()
    {
        var result = PasswordValidator.Validate("Abc123456");
        Assert.False(result.ok);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTooShort()
    {
        var result = PasswordValidator.Validate("Ab1!");
        Assert.False(result.ok);
    }
}