using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
<<<<<<< HEAD
=======
using System.IdentityModel.Tokens.Jwt;
>>>>>>> AnalisisSonarEstablishment

namespace MicroServicioUser.App.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config) => _config = config;

<<<<<<< HEAD
    public Task<Result<string>> GenerateToken(int userId, string role)
    {
        IConfigurationSection jwt = _config.GetSection("Jwt");

        string keyValue = jwt["Key"]
            ?? throw new InvalidOperationException("Jwt:Key no está configurado.");

        string issuer = jwt["Issuer"]
            ?? throw new InvalidOperationException("Jwt:Issuer no está configurado.");

        string audience = jwt["Audience"]
            ?? throw new InvalidOperationException("Jwt:Audience no está configurado.");

        string expireMinutesValue = jwt["ExpireMinutes"]
            ?? throw new InvalidOperationException("Jwt:ExpireMinutes no está configurado.");

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new()
=======
    public async Task<Result<string>> GenerateToken(int userId,  string role)
    {
        IConfigurationSection jwt = _config.GetSection("Jwt");
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new List<Claim>
>>>>>>> AnalisisSonarEstablishment
        {
            new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        JwtSecurityToken token = new JwtSecurityToken(
<<<<<<< HEAD
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(expireMinutesValue)),
            signingCredentials: creds
        );

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(Result<string>.Success(tokenValue));
=======
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpireMinutes"])),
            signingCredentials: creds
        );

        return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
>>>>>>> AnalisisSonarEstablishment
    }
}