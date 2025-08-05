using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talbat.Core.Entities;
using Talbat.Core.Entities.Identity;
using Talbat.Core.Repositories.Contract;
using Talbat.Core.Services.Contract;
using Talbat.Repository;
using Talbat.Repository.Data;
using Talbat.Repository.Identity;
using Talbat.Service;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services
            builder.Services.AddControllers();
            //Register Required Web APIs Services to the DI Container

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices(); // Custom Extension Method for Swagger

            builder.Services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
                );

            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider)=>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            }
            );

            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration); // Custom Extension Method for Identity Services
            builder.Services.AddCors(Options => 
            Options.AddPolicy("CorsPolicy", Options =>
            {
                Options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
            })
            );
            #endregion

            var app = builder.Build();

            //Ask CLR for Creating Object from Dbcontext Explicitly

            #region AlowMigraionAndSeeding
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<StoreContext>();
            var _IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync(); // Update-Database    
                await _IdentityDbContext.Database.MigrateAsync();

                await StoreContextSeed.SeedAsync(_dbContext); // Seed Data
                var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(_userManager); // Seed Identity Data
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            } 
            #endregion

            #region Configure Kestrel MiddleWares
            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExeptionMiddleware>(); // Custom Middleware for Exception Handling

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware(); // Custom Extension Method for Swagger
            }
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors();

            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run(); 
            #endregion
        }
    }
}
