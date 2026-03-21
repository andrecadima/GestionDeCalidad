using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Rules;

namespace MicroServicoUser.Inf.Repository;

public class Login : ILogin
{
    readonly IRepository _userService;
    public Login(IRepository repository)
    {
        _userService = repository;
    }

    public async Task<(bool ok, string? error)> ChangePassword(int userId, string currentPassword, string newPassword)
    {
        var userResult = await _userService.GetById(userId);
        if (!userResult.IsSuccess || userResult.Value is null)
            return (false, "Usuario no encontrado.");

        var user = userResult.Value;

        // 1) Verificar contraseña actual
        if (!RegistrationHelpers.VerifyPassword(currentPassword, user.PasswordHash))
            return (false, "La contraseña actual no es correcta.");

        // 2) Reglas de complejidad
        var pwCheck = PasswordValidator.Validate(newPassword);
        if (!pwCheck.ok)
            return (false, pwCheck.error);

        // 3) Evitar misma contraseña
        if (RegistrationHelpers.VerifyPassword(newPassword, user.PasswordHash))
            return (false, "La nueva contraseña debe ser diferente a la actual.");

        // 4) Persistir (NO tocar FirstLogin aquí)
        user.PasswordHash = RegistrationHelpers.HashPassword(newPassword);
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

        if (!RegistrationHelpers.VerifyPassword(currentPassword, user.PasswordHash))
            return (false, "La contraseña actual no es correcta.");

        if (RegistrationHelpers.VerifyPassword(newPassword, user.PasswordHash))
            return (false, "La nueva contraseña debe ser diferente a la actual.");

        user.PasswordHash = RegistrationHelpers.HashPassword(newPassword);
        user.FirstLogin = 0;
        user.LastUpdate = DateTime.UtcNow;

        var update = await _userService.Update(user);
        if (!update.IsSuccess)
            return (false, string.Join("; ", update.Errors));

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

        if (!RegistrationHelpers.VerifyPassword(plainPassword, user.PasswordHash))
            return (false, null, null, "Contraseña incorrecta.", false);

        // DB may store role as numeric code; convert it to application role name if necessary
        var roleValue = user.Role;
        if (int.TryParse(roleValue, out var code) && AuthenticationRoles.CodeToRole.ContainsKey(code))
        {
            roleValue = AuthenticationRoles.CodeToRole[code];
        }

        return (true, user.Id, roleValue, null, user.FirstLogin == 0);
    }
}