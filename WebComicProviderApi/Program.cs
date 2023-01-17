using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using WebComicProvider.Domain.Repositories.Comics;
using WebComicProvider.Domain.Repositories.Interfaces;
using WebComicProvider.Domain.Repositories.Interfaces.Users;
using WebComicProvider.Domain.Repositories.Users;
using WebComicProvider.Interfaces;
using WebComicProviderApi;
using WebComicProviderApi.Managers;
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
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearerConfiguration(sp.GetRequiredService<IConfiguration>(), sp.GetRequiredService<IUserTokenManager>());

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MultipartBoundaryLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IComicsManager, ComicsManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IComicRepository, ComicRepository>();

// add some image managers...
builder.Services.AddScoped<LocalImageManager>();

builder.Services.AddScoped<IImageManager>(m =>
{
    return m.GetRequiredService<IConfiguration>()["Storage:Mode"].ToLowerInvariant() switch
    {
        "local" => m.GetRequiredService<LocalImageManager>(),
        _ => throw new ArgumentException("Invalid Storage Mode Type"),
    };
});


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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "MyStaticFiles")),
    RequestPath = "/StaticFiles"
});
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
