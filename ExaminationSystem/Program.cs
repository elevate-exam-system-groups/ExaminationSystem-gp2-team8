
using ExaminationSystem.API.Extensions;
using ExaminationSystem.API.Middlewares;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Infrastructure.Identity;
using ExaminationSystem.Infrastructure.Persistence;
using ExaminationSystem.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExaminationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<ExamAppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );
            builder.Services.AddDbContext<IdentityDbContext<User, IdentityRole<int>, int>>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );
            builder.Services.AddAuthentication();
            builder.Services.ConfigureIdentity();
            builder.Services.AddInfrastructureServices();

            builder.Services.AddMediatR(typeof(Program).Assembly);
            builder.Services.AddScoped<ICurrentUserService,CurrentUserService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));


            var app = builder.Build();

            await app.SeedRolesAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<CustomMiddleWareHandler>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
