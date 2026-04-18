using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Infrastructure.Identity;
using ExaminationSystem.Infrastructure.Persistence;
using ExaminationSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<int>>(o =>
            {
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 8;
                o.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<IdentityStoreDbContext>()
            .AddRoles<IdentityRole<int>>()
            .AddDefaultTokenProviders();

            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            return services;
        }
        /// <summary>Seeds required roles on first run.</summary>
        public static async Task SeedRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole<int>>>();  // ← int key

            string[] roles = ["Student", "Admin"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole<int>(role));

                }
            }
        }
    }
}
