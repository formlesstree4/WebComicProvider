using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebComicProvider.Domain.Repositories.Interfaces.Users;
using WebComicProvider.Domain.Repositories.Users;
using WebComicProvider.Interfaces;
using WebComicProviderApi;
using WebComicProviderApi.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "WebComicApi";
});

// Register Managers
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IUserTokenManager, UserTokenManager>();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// build this early
var sp = builder.Services.BuildServiceProvider();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearerConfiguration(sp.GetRequiredService<IConfiguration>(), sp.GetRequiredService<IUserTokenManager>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
{
    builder.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
