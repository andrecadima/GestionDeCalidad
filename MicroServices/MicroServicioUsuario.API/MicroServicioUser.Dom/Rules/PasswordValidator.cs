using System.Text.RegularExpressions;

namespace MicroServicioUser.Dom.Rules;

<<<<<<< HEAD
public static class PasswordValidator
{
    private static readonly Regex Strong = new(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100)
=======
public class PasswordValidator
{
    private static readonly Regex Strong = new(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        RegexOptions.Compiled
>>>>>>> AnalisisSonarEstablishment
    );

    public static (bool ok, string? error) Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return (false, "La contraseña no puede estar vacía.");

<<<<<<< HEAD
        try
        {
            if (!Strong.IsMatch(password))
                return (false, "La contraseña debe tener al menos 8 caracteres e incluir al menos una mayúscula, una minúscula, un número y un carácter especial.");
        }
        catch (RegexMatchTimeoutException)
        {
            return (false, "La contraseña es inválida (timeout de validación).");
        }
=======
        if (!Strong.IsMatch(password))
            return (false, "La contraseña debe tener al menos 8 caracteres e incluir al menos una mayúscula, una minúscula, un número y un carácter especial.");
>>>>>>> AnalisisSonarEstablishment

        return (true, null);
    }
}