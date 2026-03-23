using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroServicoUser.Inf.Repository;
using Xunit;

namespace MicroServicioUser.Tests.Inf;

public class AuthenticationRolesTests
{
    [Fact]
    public void RoleToCode_ShouldMapAdminCorrectly()
    {
        Assert.Equal(1, AuthenticationRoles.RoleToCode["Admin"]);
    }

    [Fact]
    public void RoleToCode_ShouldMapContadorCorrectly()
    {
        Assert.Equal(2, AuthenticationRoles.RoleToCode["Contador"]);
    }

    [Fact]
    public void CodeToRole_ShouldMapCorrectly()
    {
        Assert.Equal("Admin", AuthenticationRoles.CodeToRole[1]);
        Assert.Equal("Contador", AuthenticationRoles.CodeToRole[2]);
    }
}