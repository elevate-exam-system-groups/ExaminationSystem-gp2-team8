using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Infrastructure.Identity;
using ExaminationSystem.Infrastructure.Persistence;
using ExaminationSystem.Infrastructure.Persistence.Data;
using ExaminationSystem.Infrastructure.Persistence.Repositories;
using ExaminationSystem.Infrastructure.Persistence.Reposteries;
using ExaminationSystem.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ExamAppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddMediatR(typeof(Program).Assembly);

            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<ExamAppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
            });

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(15);
            });

            builder.Services.AddScoped<IEmailSender, EmailSenderService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dbContext = services.GetRequiredService<ExamAppDbContext>();
                dbContext.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}