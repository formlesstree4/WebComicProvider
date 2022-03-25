using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebComicProvider;
using WebComicProvider.Authentication;
using WebComicProvider.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new HttpClient
    {
        BaseAddress = new Uri(configuration["ApiLocation"])
    };
});
builder.Services.AddScoped<WcpAuthenticationService>();
builder.Services.AddScoped<WcpAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<WcpAuthenticationStateProvider>());
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

//builder.Services.AddAuthentication();
//builder.Services.AddOidcAuthentication(options =>
//{
//    // Configure your authentication provider options here.
//    // For more information, see https://aka.ms/blazor-standalone-auth
//    builder.Configuration.Bind("Google", options.ProviderOptions);
//});

await builder.Build().RunAsync();
