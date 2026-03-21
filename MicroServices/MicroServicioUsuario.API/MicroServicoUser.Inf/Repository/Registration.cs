using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicoUser.Inf.Repository
{
    public static class RegistrationHelpers
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32;  // 256 bits
        private const int Iterations = 100000;

        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256
            );

            byte[] hash = pbkdf2.GetBytes(KeySize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('.', 3);
            if (parts.Length != 3)
                return false;

            if (!int.TryParse(parts[0], out int iterations))
                return false;

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expectedHash = Convert.FromBase64String(parts[2]);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256
            );

            byte[] actualHash = pbkdf2.GetBytes(expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }

    public class Registration : IRegistration
    {
        private readonly IRepository _userRepository;

        public Registration(IRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> GeneratePassword(int lenght)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%";
            byte[] bytes = RandomNumberGenerator.GetBytes(lenght);

            var sb = new StringBuilder();
            for (int i = 0; i < lenght; i++)
            {
                sb.Append(chars[bytes[i] % chars.Length]);
            }

            return await Task.FromResult(sb.ToString());
        }

        public async Task<(bool ok, string? generatedUsername, string? generatedPassword, string? error)> RegisterUser(
            string firstName,
            string lastName,
            string email,
            string role,
            int createdBy)
        {
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email))
            {
                return (false, null, null, "Faltan datos obligatorios.");
            }

            static string RemoveDiacritics(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return string.Empty;

                var normalized = text.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();

                foreach (var c in normalized)
                {
                    var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (uc != UnicodeCategory.NonSpacingMark)
                        sb.Append(c);
                }

                return sb.ToString().Normalize(NormalizationForm.FormC);
            }

            static string SlugifyLetters(string text)
            {
                text = RemoveDiacritics(text).ToLowerInvariant();
                return new string(text.Where(ch => ch is >= 'a' and <= 'z').ToArray());
            }

            var firstSlug = SlugifyLetters(firstName.Trim());
            var lastSlug = SlugifyLetters(lastName.Trim());

            string baseUser;
            if (!string.IsNullOrEmpty(firstSlug) && !string.IsNullOrEmpty(lastSlug))
            {
                baseUser = $"{firstSlug.Substring(0, 1)}_{lastSlug}";
            }
            else
            {
                var local = email.Split('@')[0];
                baseUser = SlugifyLetters(local);
                if (string.IsNullOrEmpty(baseUser))
                    baseUser = "user";
            }

            var candidate = baseUser;
            int suffix = 2;

            while ((await _userRepository.GetByUsername(candidate)).Value != null)
            {
                candidate = baseUser + suffix.ToString();
                suffix++;
            }

            var pwd = await GeneratePassword(10);

            var roleCode = AuthenticationRoles.RoleToCode.ContainsKey(role)
                ? AuthenticationRoles.RoleToCode[role]
                : 0;

            var user = new User
            {
                Username = candidate,
                PasswordHash = RegistrationHelpers.HashPassword(pwd),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Role = roleCode.ToString(),
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
                Status = true,
                FirstLogin = 1
            };

            var result = await _userRepository.Insert(user);
            if (!result.IsSuccess)
                return (false, null, null, result.Errors[0]);

            return (true, candidate, pwd, null);
        }
    }
}