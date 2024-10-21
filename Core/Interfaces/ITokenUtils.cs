using Core.Models;
using System.Security.Claims;

namespace Core.Interfaces
{
	public interface ITokenUtils
	{
		string GenerateToken(IEnumerable<Claim> claims);
		RefreshToken GenerateRefreshToken();
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	}
}
