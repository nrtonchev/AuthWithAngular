using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthAPI.Extentions
{
	public static class ServiceExtentions
	{
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<ApplicationContext>(opts =>
			{
				opts.UseNpgsql(config.GetConnectionString("DefaultConnection"));
			});
		}

		public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(opts =>
			{
				opts.AddPolicy("EnableCors", builder =>
				{
					builder.SetIsOriginAllowed(origin => true)
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
				});
			});
		}

		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		}

		public static void RegisterAuthServices(this IServiceCollection services, IConfiguration config)
		{
			var secretKey = config.GetSection("AppSettings").GetChildren().FirstOrDefault(x => x.Key == "Secret");

			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(opts =>
				{
					opts.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateAudience = true,
						ValidateIssuer = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = "https://localhost:5001",
						ValidAudience = "https://localhost:5001",
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey.Value))
					};
				});
		}
	}
}
