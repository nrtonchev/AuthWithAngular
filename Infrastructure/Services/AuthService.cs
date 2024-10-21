using AutoMapper;
using Core;
using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Services
{
	public class AuthService : IAuthService
	{
		public AuthService(ITokenUtils utils, ApplicationContext context, IMapper mapper)
        {
			this.utils = utils;
			this.context = context;
			this.passwordHasher = new PasswordHasher<User>();
			this.mapper = mapper;
		}

        public async Task<AuthResponse> Authenticate(AuthRequest request)
		{
			var existingUser = await context.Users.Include(r => r.RefreshTokens).FirstOrDefaultAsync(x => x.Email == request.Email);
			if (existingUser != null)
			{
				var correctPassword = passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, request.Password);
				if (correctPassword == PasswordVerificationResult.Failed)
				{
					throw new DomainException("Incorrect password!");
				}
			}
			else
			{
				throw new DomainException("User with that email does not exist!");
			}

			var claims = GenerateUserClaims(existingUser);

			var jwtToken = utils.GenerateToken(claims);
			var refreshToken = utils.GenerateRefreshToken();

			RemoveOldRefreshTokens(existingUser);
			existingUser.RefreshTokens.Add(refreshToken);
			
			context.Update(existingUser);
			await context.SaveChangesAsync();

			var response = mapper.Map<AuthResponse>(existingUser);
			response.JwtToken = jwtToken;
			response.RefreshToken = refreshToken.Token;

			return response;
		}

		public async Task<AuthResponse> RefreshToken(string token)
		{
			var existingUser = await context.Users
				.Include(r => r.RefreshTokens)
				.FirstOrDefaultAsync(x => x.RefreshTokens.Any(r => r.Token == token));

			if (existingUser == null)
			{
				throw new DomainException("Invalid refresh token!");
			}

			var refreshToken = existingUser.RefreshTokens.Single(r => r.Token == token);

			if (refreshToken.IsExpired)
			{
				throw new DomainException("Refresh token is expired!");
			}

			RemoveOldRefreshTokens(existingUser);
			var newRefreshToken = utils.GenerateRefreshToken();
			existingUser.RefreshTokens.Add(newRefreshToken);

			context.Update(existingUser);
			await context.SaveChangesAsync();
			
			var claims = GenerateUserClaims(existingUser);
			var jwtToken = utils.GenerateToken(claims);

			var response = mapper.Map<AuthResponse>(existingUser);
			response.JwtToken = jwtToken;
			response.RefreshToken = refreshToken.Token;

			return response;
		}

		#region Private members
		private void RemoveOldRefreshTokens(User user)
		{
			user.RefreshTokens.Clear();
		}

		private IEnumerable<Claim> GenerateUserClaims(User existingUser)
		{
			return new List<Claim>
			{
				new Claim(ClaimTypes.Email, existingUser.Email),
				new Claim(ClaimTypes.Role, Enum.GetName(existingUser.Role))
			};
		}

		private readonly ITokenUtils utils;
		private readonly ApplicationContext context;
		private readonly PasswordHasher<User> passwordHasher;
		private readonly IMapper mapper;
		#endregion
	}
}
