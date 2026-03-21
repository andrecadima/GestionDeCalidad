using System.Text.RegularExpressions;

namespace MicroServicioUser.Dom.Rules;

public static class PasswordValidator
{
    private static readonly Regex Strong = new(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100)
    );

    public static (bool ok, string? error) Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return (false, "La contraseña no puede estar vacía.");

        try
        {
            if (!Strong.IsMatch(password))
                return (false, "La contraseña debe tener al menos 8 caracteres e incluir al menos una mayúscula, una minúscula, un número y un carácter especial.");
        }
        catch (RegexMatchTimeoutException)
        {
            return (false, "La contraseña es inválida (timeout de validación).");
        }

        return (true, null);
    }
}