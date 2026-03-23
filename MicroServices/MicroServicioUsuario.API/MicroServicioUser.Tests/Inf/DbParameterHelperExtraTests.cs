using MicroServicoUser.Inf.Persistence.Database;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class DbParameterHelperExtraTests
{
    private class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Juan";
    }

    [Fact]
    public void PopulateCommandParameters_ShouldReturnEmptyCommand_WhenModelIsNull()
    {
        var cmd = DbParameterHelper.PopulateCommandParameters<TestModel>("SELECT * FROM t WHERE Id=@Id", null!);

        Assert.Empty(cmd.Parameters);
    }

    [Fact]
    public void PopulateCommandParameters_ShouldIgnoreMissingProperty()
    {
        var model = new TestModel { Id = 1, Name = "Juan" };

        var cmd = DbParameterHelper.PopulateCommandParameters(
            "SELECT * FROM t WHERE Id=@Id AND Unknown=@Unknown", model);

        Assert.Single(cmd.Parameters);
        Assert.Equal("@Id", cmd.Parameters[0].ParameterName);
    }
}