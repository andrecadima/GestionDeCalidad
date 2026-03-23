using System.Data;
using MicroServicoUser.Inf.Persistence.Database;
using MicroServicioUser.Dom.Entities;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class DbMapperTests
{
    [Fact]
    public void MapDataRowToModel_ShouldMapCorrectly()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Username", typeof(string));

        var row = table.NewRow();
        row["Id"] = 1;
        row["Username"] = "test";

        table.Rows.Add(row);

        var result = DbMapper.MapDataRowToModel<User>(row);

        Assert.Equal(1, result.Id);
        Assert.Equal("test", result.Username);
    }
}