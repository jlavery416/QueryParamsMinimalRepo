using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QueryParamsMinimalRepo.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMsalAuthentication(options => {
  builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
  String[]? scopes = builder.Configuration.GetSection("AzureAd:DefaultAccessTokenScopes").Get<String[]>() ?? new String[] { };
  foreach (String scope in scopes) {
    options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
  }
  options.ProviderOptions.LoginMode = builder.Configuration["AzureAd:LoginMode"]!;
});

await builder.Build().RunAsync();
