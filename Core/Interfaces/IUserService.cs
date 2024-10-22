using Core.DTOs;

namespace Core.Interfaces
{
	public interface IUserService
	{
		Task RegisterUser(RegisterRequest request);
		Task<UserDto> CreateUser(CreateUserDto userDto);
		Task<UserDto> UpdateUser(int id, UpdateUserDto userDto);
		Task DeleteUser(int id);
		Task<List<UserDto>> GetAllUsers();
		Task<UserDto> GetUserById(int id);
	}
}
