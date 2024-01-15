using AffiliatePMS.Infra.Security;
using AffiliatePMS.Infra.Security.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AffiliatePMS.WebAPI.Controllers
{

    [ApiVersion(1.0)]
    [Route("api/user")]
    [Tags(" Authentication")]
    [ApiController]
    public class AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        // POST: auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            var (tokenCredential, errorMessage) = await _authService.LoginAsync(user.Email, user.Password);

            if (tokenCredential != null)
            {
                logger.LogInformation("User {email} logged in successfully", user.Email);
                return Ok(tokenCredential);
            }

            logger.LogWarning("User {email} login unsuccessful. Reason: {reason}", user.Email, errorMessage);
            return BadRequest(new { message = "User login unsuccessful.", reason = errorMessage });
        }

        // POST: auth/register
        [AllowAnonymous]
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            var response = await _authService.RegisterAsync(user.FullName, user.Email, user.Password);

            if (response!.IsSuccess)
            {
                var registeredUser = response.RegisteredUser;
                logger.LogInformation("User {email} registered successfully ", user.Email);
                return Accepted(new { registeredUser });
            }
            else
            {
                logger.LogWarning(response.ErrorMessage);
                return BadRequest(new { message = response.ErrorMessage });
            }
        }
    }
}
