using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebComicProvider;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddAuthentication();
builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Local", options.ProviderOptions);

});
//builder.Services.AddCors(policy =>
//{
//    policy.AddPolicy("_myAllowSpecificOrigins", builder => builder.WithOrigins("http://external:9000/")
//         .AllowAnyMethod()
//         .AllowAnyHeader()
//         .AllowCredentials());
//});



await builder.Build().RunAsync();
