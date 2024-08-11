using KoperasiTentera.API.Common;
using KoperasiTentera.Application.ApplicationServices;
using KoperasiTentera.Application.Interfaces;
using KoperasiTentera.Infrastructure;
using KoperasiTentera.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.

#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddControllers()
        .AddFluentValidation(configurationExpression: fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("KoperasiTentera.Application")));
#pragma warning restore CS0618 // Type or member is obsolete
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOTPService, OtpService>();
builder.Services.AddSingleton<IOTPStore, OTPStore>();
builder.Services.AddDbContext<KoperasiTenteraDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLLiteConnection")));
// Add other necessary services
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
