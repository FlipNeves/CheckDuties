using CheckDuties.Domain.Entities;
using CheckDuties.Domain.Interfaces.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace CheckDuties.App.Commands.UsersCommand.RegisterUserCommand;

public class RegisterUserCommandHandler
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<bool> Handle(string username, string password)
    {
        var existingUser = await _userRepository.GetAllAsync();
        var existingUniqueUser = existingUser.Where(u => u.Username == username).FirstOrDefault();
        if (existingUniqueUser?.Id != null)
            return false;

        var hashedPassword = HashPassword(password);
        var user = new User(username, hashedPassword);
        await _userRepository.AddAsync(user);
        return true;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}