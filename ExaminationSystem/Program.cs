using ExaminationSystem.API.Extensions;
using ExaminationSystem.API.Middlewares;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Infrastructure.Identity;
using ExaminationSystem.Infrastructure.Persistence;
using ExaminationSystem.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ExaminationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ExamAppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<ExamAppDbContext>()
                .AddDefaultTokenProviders();



            builder.Services.AddInfrastructureServices();

            builder.Services.AddMediatR(typeof(Program).Assembly);

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
            builder.Services.AddScoped<IDataseeding, Dataseeding>();
            builder.Services.AddMemoryCache();
            var app = builder.Build();

            //using var scope = app.Services.CreateScope();
            //var seeder = scope.ServiceProvider.GetRequiredService<IDataseeding>();
            //await seeder.SeedDataAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.SeedRolesAsync();

            app.Run();
        }
    }
}