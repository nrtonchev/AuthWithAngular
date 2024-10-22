using AutoMapper;
using Core;
using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
	public class UserService : IUserService
	{
		public UserService(ApplicationContext context, IMapper mapper)
        {
			this.passwordHasher = new PasswordHasher<User>();
			this.context = context;
			this.mapper = mapper;
		}
        public async Task RegisterUser(RegisterRequest request)
		{
			var existing = await context.Users.AnyAsync(x => x.Email == request.Email);
			if (existing)
			{
				throw new DomainException("User with that email is already registered");
			}

			var user = mapper.Map<User>(request);

			user.Role = Role.User;
			user.DateCreated = DateTime.UtcNow;
			user.DateUpdated = DateTime.UtcNow;
			user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

			await context.Users.AddAsync(user);
			await context.SaveChangesAsync();
		}

		#region Private members
		private readonly PasswordHasher<User> passwordHasher;
		private readonly ApplicationContext context;
		private readonly IMapper mapper;
		#endregion
	}
}
