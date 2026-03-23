<<<<<<< HEAD
﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> AnalisisSonarEstablishment

namespace MicroServicoUser.Inf.Repository
{
    public static class AuthenticationRoles
    {
<<<<<<< HEAD
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
=======
        public static readonly Dictionary<string, int> RoleToCode = new()
        {
            { "Admin", 1 },
            { "Contador", 2 }
        };

        public static readonly Dictionary<int, string> CodeToRole = RoleToCode.ToDictionary(kv => kv.Value, kv => kv.Key);
    }
}
>>>>>>> AnalisisSonarEstablishment
