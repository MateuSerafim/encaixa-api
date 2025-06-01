using BaseRepository.UnitWorkBase;
using Encaixa.Application.Orquestrators;
using Encaixa.Application.Services.Users;
using Encaixa.Infrastructure.Context;
using Encaixa.Infrastructure.UserIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EncaixaAPI.Configurations;
public static class BuilderConfiguration
{
    public static WebApplication BuildWebApplication(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();

        builder.ConfigureContext();
        builder.ConfigureServices();

        return builder.Build();
    }

    public static void ConfigureContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<EncaixaContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<UserApplication, IdentityRole>()
                        .AddEntityFrameworkStores<EncaixaContext>()
                        .AddDefaultTokenProviders();

        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<EncaixaContext>());
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrchestrator, Orchestrator>();
        
        builder.Services.AddScoped<IUserService, UserService>();
    }
}
