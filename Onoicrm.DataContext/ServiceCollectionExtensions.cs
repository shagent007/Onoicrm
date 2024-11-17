using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Onoicrm.DataContext.Services;
using Onoicrm.Domain.Services;

namespace Onoicrm.DataContext;


public static class ServiceCollectionExtensions
{
    public static void RegisterEfServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ApplicationDataContext>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IFileService, FileService>();
    }

    public static void SetupEfInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 5;
            })
            .AddEntityFrameworkStores<ApplicationDataContext>()
            .AddDefaultTokenProviders();
        

        
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = "issuer",
                ValidAudience = "audience",
                IssuerSigningKey = new SymmetricSecurityKey("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"u8.ToArray())
            };
        });
        
        serviceCollection.AddDbContext<ApplicationDataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}

