using CheckDuties.Domain.Entities;
using CheckDuties.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CheckDuties.App.Commands.UsersCommand.LoginUserCommand;

public class LoginUserCommandHandler
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(string username, string password)
    {
        var user = await _userRepository.GetAllAsync();
        var uniqueUser = user.Where(u => u.Username == username).FirstOrDefault();

        if (uniqueUser == null || !VerifyPassword(password, uniqueUser.PasswordHash))
            return "Inválido";

        return GenerateJwtToken(uniqueUser);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hashedPassword = Convert.ToBase64String(hashedBytes);
        return hashedPassword == storedHash;
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSecret"]);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}