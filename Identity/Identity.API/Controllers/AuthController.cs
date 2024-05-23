using Identity.API.Data;
using Identity.Common;
using Identity.Service.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("v2/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService authService;
	private readonly ILogger<AuthController> logger;

	public AuthController(IAuthService authService, ILogger<AuthController> logger)
	{
		this.authService = authService;
		this.logger = logger;
	}

	[HttpPost("user")]
	public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequestDto request)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var registerResult = await authService.RegisterUserAsync(request.User, request.Password);
			if (registerResult.Status)
			{
				return Ok(new { Message = "User registered successfully" });
			}

			return BadRequest(new { Message = registerResult.Errors });
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred during user registration");
			return StatusCode(500, new { Message = "Internal server error" });
		}
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserRequestDto request)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var authResult = await authService.AuthenticateAsync(request.Login, request.Password);
			if (authResult.Status)
			{
				return Ok(new { Message = "Login successful" });
			}

			return Unauthorized(new { Message = "Invalid username or password" });
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred during user login");
			return StatusCode(500, new { Message = "Internal server error" });
		}
	}

	[HttpPost("logout")]
	public async Task<IActionResult> LogoutUserAsync()
	{
		try
		{
			bool logoutSuccessful = await authService.LogoutAsync();
			if (logoutSuccessful)
			{
				return Ok(new { Message = "Logout successful" });
			}
			else
			{
				return BadRequest(new { Message = "Logout failed" });
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred during user logout");
			return StatusCode(500, new { Message = "Internal server error" });
		}
	}

	[HttpGet("user/{username}")]
	public async Task<IActionResult> GetUserAsync([FromRoute] string username)
	{
		try
		{
			var user = await authService.GetUserDataAsync(username);
			if (user != null)
			{
				return Ok(user);
			}
			else
			{
				return NotFound(new { Message = "User not found" });
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred while retrieving user data");
			return StatusCode(500, new { Message = "Internal server error" });
		}
	}
}

public record RegisterUserRequestDto(RegisterUserData User, string Password);

public record LoginUserRequestDto(string Login, string Password);