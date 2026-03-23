using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Rules;

namespace MicroServicoUser.Inf.Repository;

public class Login : ILogin
{
<<<<<<< HEAD
    readonly IRepository _userService;
=======
    IRepository _userService;
>>>>>>> AnalisisSonarEstablishment
    public Login(IRepository repository)
    {
        _userService = repository;
    }

    public async Task<(bool ok, string? error)> ChangePassword(int userId, string currentPassword, string newPassword)
    {
<<<<<<< HEAD
=======
        
>>>>>>> AnalisisSonarEstablishment
        var userResult = await _userService.GetById(userId);
        if (!userResult.IsSuccess || userResult.Value is null)
            return (false, "Usuario no encontrado.");

        var user = userResult.Value;

        // 1) Verificar contraseña actual
<<<<<<< HEAD
        if (!RegistrationHelpers.VerifyPassword(currentPassword, user.PasswordHash!))
=======
        var currentHash = RegistrationHelpers.Md5Hex(currentPassword);
        if (!string.Equals(user.PasswordHash, currentHash, StringComparison.OrdinalIgnoreCase))
>>>>>>> AnalisisSonarEstablishment
            return (false, "La contraseña actual no es correcta.");

        // 2) Reglas de complejidad
        var pwCheck = PasswordValidator.Validate(newPassword);
        if (!pwCheck.ok)
            return (false, pwCheck.error);

        // 3) Evitar misma contraseña
<<<<<<< HEAD
        if (RegistrationHelpers.VerifyPassword(newPassword, user.PasswordHash!))
            return (false, "La nueva contraseña debe ser diferente a la actual.");

        // 4) Persistir (NO tocar FirstLogin aquí)
        user.PasswordHash = RegistrationHelpers.HashPassword(newPassword);
=======
        var newHash = RegistrationHelpers.Md5Hex(newPassword);
        if (string.Equals(user.PasswordHash, newHash, StringComparison.OrdinalIgnoreCase))
            return (false, "La nueva contraseña debe ser diferente a la actual.");

        // 4) Persistir (NO tocar FirstLogin aquí)
        user.PasswordHash = newHash;
>>>>>>> AnalisisSonarEstablishment
        user.LastUpdate = DateTime.UtcNow;

        var update = await _userService.Update(user);
        if (!update.IsSuccess)
            return (false, string.Join("; ", update.Errors));

        return (true, null);
    }

    public async Task<(bool ok, string? error)> ChangePasswordFirstLogin(int userId, string currentPassword, string newPassword)
    {
        var userResult = await _userService.GetById(userId);
        if (!userResult.IsSuccess || userResult.Value is null)
            return (false, "Usuario no encontrado.");

        var user = userResult.Value;

        if (user.FirstLogin != 1)
            return (false, "Este usuario ya ha cambiado su contraseña inicial.");

<<<<<<< HEAD
        if (!RegistrationHelpers.VerifyPassword(currentPassword, user.PasswordHash!))
            return (false, "La contraseña actual no es correcta.");

        if (RegistrationHelpers.VerifyPassword(newPassword, user.PasswordHash!))
            return (false, "La nueva contraseña debe ser diferente a la actual.");

        user.PasswordHash = RegistrationHelpers.HashPassword(newPassword);
=======
        var currentHash = RegistrationHelpers.Md5Hex(currentPassword);
        if (!string.Equals(user.PasswordHash, currentHash, StringComparison.OrdinalIgnoreCase))
            return (false, "La contraseña actual no es correcta.");

   
        var newHash = RegistrationHelpers.Md5Hex(newPassword);
        if (string.Equals(user.PasswordHash, newHash, StringComparison.OrdinalIgnoreCase))
            return (false, "La nueva contraseña debe ser diferente a la actual.");
        user.PasswordHash = newHash;
>>>>>>> AnalisisSonarEstablishment
        user.FirstLogin = 0;
        user.LastUpdate = DateTime.UtcNow;

        var update = await _userService.Update(user);
        if (!update.IsSuccess)
            return (false, string.Join("; ", update.Errors));
<<<<<<< HEAD

        return (true, null);
    }

    public async Task<(bool ok, int? userId, string? role, string? error, bool isFirstLogin)> ValidateLogin(
        string username,
        string plainPassword)
    {
        var result = await _userService.GetByUsername(username);
        if (result.IsFailure || result.Value == null || !result.Value.Status)
            return (false, null, null, result.Errors.FirstOrDefault() ?? "Usuario no encontrado.", false);

        var user = result.Value;

        if (!RegistrationHelpers.VerifyPassword(plainPassword, user.PasswordHash!))
=======
        return (true, null);
    }

    public async Task<(bool ok, int? userId, string? role, string? error, bool isFirstLogin)> ValidateLogin(string username,
        string plainPassword)
    {
        var result = await _userService.GetByUsername(username); //tienes
        if(result.IsFailure || !result.Value.Status) return (false, null, null, result.Errors.First(), false);
        
        var user = result.Value;
        var givenHash = RegistrationHelpers.Md5Hex(plainPassword);
        if (!string.Equals(user.PasswordHash, givenHash, System.StringComparison.OrdinalIgnoreCase))
>>>>>>> AnalisisSonarEstablishment
            return (false, null, null, "Contraseña incorrecta.", false);

        // DB may store role as numeric code; convert it to application role name if necessary
        var roleValue = user.Role;
        if (int.TryParse(roleValue, out var code) && AuthenticationRoles.CodeToRole.ContainsKey(code))
        {
            roleValue = AuthenticationRoles.CodeToRole[code];
        }
<<<<<<< HEAD

=======
>>>>>>> AnalisisSonarEstablishment
        return (true, user.Id, roleValue, null, user.FirstLogin == 0);
    }
}