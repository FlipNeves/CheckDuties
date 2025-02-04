using CheckDuties.App.Commands.UsersCommand.LoginUserCommand;
using CheckDuties.App.Commands.UsersCommand.RegisterUserCommand;
using CheckDuties.App.UsualDto;
using Microsoft.AspNetCore.Mvc;

namespace CheckDuties.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly RegisterUserCommandHandler _registerUserHandler;
        private readonly LoginUserCommandHandler _loginUserHandler;

        public UsersController(
            ILogger<UsersController> logger,
            RegisterUserCommandHandler registerUserHandler,
            LoginUserCommandHandler loginUserHandler)
        {
            _logger = logger;
            _registerUserHandler = registerUserHandler;
            _loginUserHandler = loginUserHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                await _registerUserHandler.Handle(userDto.Username, userDto.Password);
                return Ok(new { message = "Usuário registrado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            try
            {
                var token = await _loginUserHandler.Handle(userDto.Username, userDto.Password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
