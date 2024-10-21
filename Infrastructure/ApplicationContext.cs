using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions options) : base(options)
		{
		}

        public DbSet<User> Users { get; set; }
    }
}
