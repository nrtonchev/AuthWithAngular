using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions options) : base(options)
		{
			this.passwordHasher = new PasswordHasher<User>();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			var adminUser = new User
			{
				Id = 1,
				FirstName = "Admin",
				LastName = "Admin",
				Email = "admin@admin.com",
				Role = Role.Administrator,
				DateCreated = DateTime.UtcNow,
				DateUpdated = DateTime.UtcNow
			};

			adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");

			modelBuilder.Entity<User>().HasData(adminUser);
		}

		public DbSet<User> Users { get; set; }

		#region Private members
		private readonly PasswordHasher<User> passwordHasher;
		#endregion
	}
}
