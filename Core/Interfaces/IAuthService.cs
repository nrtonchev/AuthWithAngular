using Core.DTOs;

namespace Core.Interfaces
{
	public interface IAuthService
	{
		Task<AuthResponse> Authenticate(AuthRequest request);
		Task<AuthResponse> RefreshToken(string token);
	}
}
