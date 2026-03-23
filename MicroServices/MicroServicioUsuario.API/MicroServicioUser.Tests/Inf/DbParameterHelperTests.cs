using MicroServicoUser.Inf.Persistence.Database;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class DbParameterHelperTests
{
    private class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "test";
    }

    [Fact]
    public void PopulateCommandParameters_ShouldAddParameters()
    {
        var query = "SELECT * FROM test WHERE Id = @Id AND Name = @Name";
        var model = new TestModel { Id = 1, Name = "Juan" };

        var cmd = DbParameterHelper.PopulateCommandParameters(query, model);

        Assert.Equal(2, cmd.Parameters.Count);
    }
}