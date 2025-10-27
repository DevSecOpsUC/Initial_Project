using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(p => p.AddDefaultPolicy(b => b
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();
app.UseCors();

app.MapPost("/api/auth/login", async (HttpContext context) =>
{
    // Leer manualmente el cuerpo del request
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();

    // Intentar deserializar
    var login = JsonSerializer.Deserialize<LoginRequest>(body, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true // 👈 Ignora mayúsculas/minúsculas
    });

    // Validar credenciales
    if (login is not null && login.Username == "admin" && login.Password == "1234")
    {
        await context.Response.WriteAsJsonAsync(new { token = "fake-jwt-123", user = login.Username });
    }
    else
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "Credenciales inválidas" });
    }
});

app.Run();

record LoginRequest(string Username, string Password);
