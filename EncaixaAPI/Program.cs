using EncaixaAPI.Configurations;
using EncaixaAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.BuildWebApplication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.RegisterUsersEndpoints();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();
