using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors();

app.MapGet("/api/rooms", () =>
{
    var rooms = new[]
    {
        new { name = "Habitación 101", price = 680000 },
        new { name = "Habitación 202", price = 550000 },
        new { name = "Habitación 303", price = 750000 },
    };
    return Results.Json(rooms);
});

app.Run();
