using AuthAPI.Extentions;
using AuthAPI.Helpers;
using Core.Models;
using System.Text.Json.Serialization;

namespace AuthAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.ConfigureDbContext(builder.Configuration);
			builder.Services.ConfigureCors();
			builder.Services.RegisterServices();
			builder.Services.RegisterAuthServices(builder.Configuration);
			builder.Services.AddControllers()
				.AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
			builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseCors("EnableCors");

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseMiddleware<GlobalErrorHandler>();

			app.MapControllers();

			app.Run();
		}
	}
}
