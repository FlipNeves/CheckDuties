using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CheckDuties.Data.Context;
using CheckDuties.App.Commands.UsersCommand.RegisterUserCommand;
using CheckDuties.App.Commands.UsersCommand.LoginUserCommand;
using CheckDuties.Domain.Interfaces.Repositories;
using CheckDuties.Data.Repositories;
using MongoDB.Driver;

namespace CheckDuties;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IMongoClient>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"]));
            return new MongoClient(settings);
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IDutyRepository, DutyRepository>();
        builder.Services.AddScoped<RegisterUserCommandHandler>();
        builder.Services.AddScoped<LoginUserCommandHandler>();

        var jwtKey = builder.Configuration["JwtSecret"];
        var key = Encoding.ASCII.GetBytes(jwtKey);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        var app = builder.Build();

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
