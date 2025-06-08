using System.Text;
using BaseRepository.UnitWorkBase;
using Encaixa.Application.Orquestrators;
using Encaixa.Application.Services.Packages;
using Encaixa.Application.Services.Users;
using Encaixa.Infrastructure.Context;
using Encaixa.Infrastructure.UserIdentity;
using EncaixaAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace EncaixaAPI.Configurations;
public static class BuilderConfiguration
{
    public static WebApplication BuildWebApplication(this WebApplicationBuilder builder)
    {
        builder.ConfigureSwaggerGeneration();
        builder.ConfigureDatabaseContext();
        builder.ConfigureAuthentication();
        builder.ConfigureIdentityServices();
        builder.ConfigureServices();

        return builder.Build();
    }

    public static void ConfigureSwaggerGeneration(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Encaixa API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header. Digite: **Bearer {seu_token}**",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
    
    public static void ConfigureDatabaseContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<EncaixaContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<EncaixaContext>());
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JwtSettings"));
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JWTSettings>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                LogValidationExceptions = true
            };
        });

        builder.Services.AddAuthorization();

        builder.Services.AddRouting();
    }

    public static void ConfigureIdentityServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityCore<UserApplication>()
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<EncaixaContext>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
        });
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrchestrator, Orchestrator>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IPackageBoxService, PackageBoxService>();

        builder.Services.AddScoped<UserReference>();
    }
}
