using Core;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
	public class TokenUtils : ITokenUtils
	{
		private readonly AppSettings appSettings;
		private readonly ApplicationContext context;

		public TokenUtils(IOptions<AppSettings> appSettings, ApplicationContext context)
        {
			this.appSettings = appSettings.Value;
			this.context = context;
		}

        public RefreshToken GenerateRefreshToken()
		{
			var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
			var isExistingToken = context.Users.Any(x => x.RefreshTokens.Any(r => r.Token == token));

			if (isExistingToken)
			{
				return GenerateRefreshToken();
			}

			var refreshToken = new RefreshToken
			{
				Token = token,
				DateCreated = DateTime.UtcNow,
				ExpiryDate = DateTime.UtcNow.AddDays(appSettings.RefreshTokenExpiry)
			};

			return refreshToken;
		}

		public string GenerateToken(IEnumerable<Claim> claims)
		{
			var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret));
			var signInCreds = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

			var tokenOptions = new JwtSecurityToken(
				issuer: appSettings.Issuer,
				audience: appSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(appSettings.TokenExpiry),
				signingCredentials: signInCreds
			);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
			return tokenString;
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret)),
				ValidateLifetime = false,
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;

			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new DomainException("Invalid token");
			}

			return principal;
		}
	}
}
