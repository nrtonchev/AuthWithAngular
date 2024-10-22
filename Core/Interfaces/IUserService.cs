using Core.DTOs;

namespace Core.Interfaces
{
	public interface IUserService
	{
		Task RegisterUser(RegisterRequest request);
	}
}
