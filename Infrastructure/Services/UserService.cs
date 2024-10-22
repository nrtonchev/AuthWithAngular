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

		public async Task<UserDto> CreateUser(CreateUserDto userDto)
		{
			var existingEmail = await context.Users.AnyAsync(x => x.Email == userDto.Email);

			if (existingEmail)
			{
				throw new DomainException("User with same email already exists!");
			}

			if (userDto.Role == Role.Administrator)
			{
				throw new DomainException("User cannot have administrative rights!");
			}

			var user = mapper.Map<User>(userDto);

			user.PasswordHash = passwordHasher.HashPassword(user, userDto.Password);
			user.DateCreated = DateTime.UtcNow;
			user.DateUpdated = DateTime.UtcNow;

			await context.Users.AddAsync(user);
			await context.SaveChangesAsync();

			return mapper.Map<UserDto>(user);
		}

		public async Task<UserDto> UpdateUser(int id, UpdateUserDto userDto)
		{
			var existingEmail = await context.Users.AnyAsync(x => x.Email == userDto.Email && x.Id != id);

			if (existingEmail)
			{
				throw new DomainException("User with same email already exists!");
			}

			var existingUser = await context.Users.FindAsync(id);

			if (existingUser is null)
			{
				throw new DomainException("User with that id is not found in database!");
			}

			if (userDto.Role == Role.Administrator)
			{
				throw new DomainException("User cannot have administrative rights!");
			}

			if (!string.IsNullOrEmpty(userDto.Password))
			{
				existingUser.PasswordHash = passwordHasher.HashPassword(existingUser, userDto.Password);
			}

			mapper.Map(userDto, existingUser);

			existingUser.DateUpdated = DateTime.UtcNow;

			context.Update(existingUser);
			await context.SaveChangesAsync();

			return mapper.Map<UserDto>(existingUser);
		}

		public async Task DeleteUser(int id)
		{
			var existingUser = await context.Users.FindAsync(id);
			if (existingUser is null)
			{
				throw new DomainException("User with that id is not found in database!");
			}

			context.Remove(existingUser);
			await context.SaveChangesAsync();
		}

		public async Task<List<UserDto>> GetAllUsers()
		{
			var users = await context.Users
				.Select(x => mapper.Map<UserDto>(x))
				.AsNoTracking()
				.ToListAsync();

			return users;
		}

		public async Task<UserDto> GetUserById(int id)
		{
			var user = await context.Users.FindAsync(id);
			if (user is null)
			{
				throw new DomainException("User with that id is not found in database!");
			}

			return mapper.Map<UserDto>(user);
		}

		#region Private members
		private readonly PasswordHasher<User> passwordHasher;
		private readonly ApplicationContext context;
		private readonly IMapper mapper;
		#endregion
	}
}
