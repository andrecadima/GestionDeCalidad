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
<<<<<<< HEAD
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
=======
        public static string Md5Hex(string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
>>>>>>> AnalisisSonarEstablishment
        }
    }

    public class Registration : IRegistration
    {
        private readonly IRepository _userRepository;

        public Registration(IRepository userRepository)
        {
<<<<<<< HEAD
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

=======

            _userRepository = userRepository;
        }

        public async Task<string> GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%";
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[bytes[i] % chars.Length]);
            }
            return sb.ToString();
        }

        public async Task<(bool ok, string? generatedUsername, string? generatedPassword, string? error)> RegisterUser(string firstName, string lastName, string email, string role, int createdBy)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email))
                return (false, null, null, "Faltan datos obligatorios.");

            static string RemoveDiacritics(string text)
            {
                if (string.IsNullOrWhiteSpace(text)) return string.Empty;
                var normalized = text.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();
                foreach (var c in normalized)
                {
                    var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (uc != UnicodeCategory.NonSpacingMark) sb.Append(c);
                }
>>>>>>> AnalisisSonarEstablishment
                return sb.ToString().Normalize(NormalizationForm.FormC);
            }

            static string SlugifyLetters(string text)
            {
                text = RemoveDiacritics(text).ToLowerInvariant();
<<<<<<< HEAD
                return new string(text.Where(ch => ch is >= 'a' and <= 'z').ToArray());
            }

            var firstSlug = SlugifyLetters(firstName.Trim());
            var lastSlug = SlugifyLetters(lastName.Trim());
=======
                var sb = new StringBuilder(text.Length);
                foreach (var ch in text)
                {
                    if (ch is >= 'a' and <= 'z') sb.Append(ch);
                }
                return sb.ToString();
            }

            var firstSlug = SlugifyLetters(firstName?.Trim() ?? "");
            var lastSlug = SlugifyLetters(lastName?.Trim() ?? "");
>>>>>>> AnalisisSonarEstablishment

            string baseUser;
            if (!string.IsNullOrEmpty(firstSlug) && !string.IsNullOrEmpty(lastSlug))
            {
                baseUser = $"{firstSlug.Substring(0, 1)}_{lastSlug}";
            }
            else
            {
<<<<<<< HEAD
                var local = email.Split('@')[0];
                baseUser = SlugifyLetters(local);
                if (string.IsNullOrEmpty(baseUser))
                    baseUser = "user";
=======
                var local = (email ?? "").Split('@')[0];
                baseUser = SlugifyLetters(local);
                if (string.IsNullOrEmpty(baseUser)) baseUser = "user";
>>>>>>> AnalisisSonarEstablishment
            }

            var candidate = baseUser;
            int suffix = 2;
<<<<<<< HEAD

            while ((await _userRepository.GetByUsername(candidate)).Value != null)
=======
            while ((await _userRepository.GetByUsername(candidate)).Value!=null)
>>>>>>> AnalisisSonarEstablishment
            {
                candidate = baseUser + suffix.ToString();
                suffix++;
            }

            var pwd = await GeneratePassword(10);

<<<<<<< HEAD
            var roleCode = AuthenticationRoles.RoleToCode.ContainsKey(role)
                ? AuthenticationRoles.RoleToCode[role]
                : 0;
=======
            
            var roleCode = AuthenticationRoles.RoleToCode.ContainsKey(role) ? AuthenticationRoles.RoleToCode[role] : 0;
>>>>>>> AnalisisSonarEstablishment

            var user = new User
            {
                Username = candidate,
<<<<<<< HEAD
                PasswordHash = RegistrationHelpers.HashPassword(pwd),
=======
                PasswordHash = RegistrationHelpers.Md5Hex(pwd),
>>>>>>> AnalisisSonarEstablishment
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Role = roleCode.ToString(),
                CreatedBy = createdBy,
<<<<<<< HEAD
                CreatedDate = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
=======
                CreatedDate = System.DateTime.UtcNow,
                LastUpdate = System.DateTime.UtcNow,
>>>>>>> AnalisisSonarEstablishment
                Status = true,
                FirstLogin = 1
            };

            var result = await _userRepository.Insert(user);
            if (!result.IsSuccess)
<<<<<<< HEAD
                return (false, null, null, result.Errors[0]);
=======
                return (false, null, null,result.Errors.First());
>>>>>>> AnalisisSonarEstablishment

            return (true, candidate, pwd, null);
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> AnalisisSonarEstablishment
