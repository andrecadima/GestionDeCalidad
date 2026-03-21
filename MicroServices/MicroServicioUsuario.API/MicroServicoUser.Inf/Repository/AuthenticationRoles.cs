using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MicroServicoUser.Inf.Repository
{
    public static class AuthenticationRoles
    {
        public static IReadOnlyDictionary<string, int> RoleToCode { get; } =
            new ReadOnlyDictionary<string, int>(
                new Dictionary<string, int>
                {
                    { "Admin", 1 },
                    { "Contador", 2 }
                });

        public static IReadOnlyDictionary<int, string> CodeToRole { get; } =
            new ReadOnlyDictionary<int, string>(
                RoleToCode.ToDictionary(kv => kv.Value, kv => kv.Key));
    }
}