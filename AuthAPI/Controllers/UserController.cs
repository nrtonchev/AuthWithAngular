using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService userService;

		public UserController(IUserService userService)
        {
			this.userService = userService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			await userService.RegisterUser(request);
			return Ok(new { message = "Registration was successful!" });
		}

		[Authorize(Roles = "Administrator")]
		[HttpGet]
		public async Task<ActionResult<List<UserDto>>> GetUsers()
		{
			var users = await userService.GetAllUsers();
			return Ok(users);
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<ActionResult<UserDto>> GetUserById(int id)
		{
			var user = await userService.GetUserById(id);
			return Ok(user);
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost("create")]
		public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto userDto)
		{
			var user = await userService.CreateUser(userDto);
			return Ok(user);
		}

		[HttpPut("update/{id}")]
		public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
		{
			var user = await userService.UpdateUser(id, userDto);
			return Ok(user);
		}

		[Authorize(Roles = "Administrator")]
		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			await userService.DeleteUser(id);
			return Ok(new { message = "User was successfully deleted!"});
		}
	}
}
