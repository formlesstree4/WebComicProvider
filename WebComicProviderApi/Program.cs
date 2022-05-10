using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebComicProvider.Domain.Repositories.Comics;
using WebComicProvider.Domain.Repositories.Interfaces;
using WebComicProvider.Domain.Repositories.Interfaces.Users;
using WebComicProvider.Domain.Repositories.Users;
using WebComicProvider.Interfaces;
using WebComicProviderApi;
using WebComicProviderApi.Managers.Comics;
using WebComicProviderApi.Managers.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register Managers
builder.Services.AddScoped<IUserTokenManager, UserTokenManager>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "WebComicApi";
});

// build this early
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
var sp = builder.Services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IComicsManager, ComicsManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IComicRepository, ComicRepository>();

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
