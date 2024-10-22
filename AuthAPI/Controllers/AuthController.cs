using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		public AuthController(IAuthService authService)
        {
			this.authService = authService;
		}

		[HttpPost("authenticate")]
		public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
		{
			var response = await authService.Authenticate(request);
			SetTokenCookie(response.RefreshToken);
			return Ok(response);
		}

		[HttpPost("refresh-token")]
		public async Task<ActionResult<AuthResponse>> RefreshToken()
		{
			var token = Request.Cookies["refreshToken"];
			var response = await authService.RefreshToken(token);
			SetTokenCookie(response.RefreshToken);
			return Ok(response);
		}

		#region Private members
		private void SetTokenCookie(string refreshToken)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTime.UtcNow.AddDays(2)
			};

			Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}

		private readonly IAuthService authService;
		#endregion
	}
}
