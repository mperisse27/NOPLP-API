using Microsoft.EntityFrameworkCore;
using NOPLP_API.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()   // ⚠️ À éviter en production !
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDbContext<NoplpDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SongDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
