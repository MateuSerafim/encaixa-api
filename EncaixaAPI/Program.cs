using EncaixaAPI.Configurations;
using EncaixaAPI.Endpoints;
using EncaixaAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

var app = builder.BuildWebApplication();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Encaixa API v1");
    c.RoutePrefix = "";
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuthMiddleware>();

app.RegisterUsersEndpoints();
app.RegisterPackagesEndpoints();

app.Run();
