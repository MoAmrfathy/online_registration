using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;
using Shared.Global;
using UI;
using UI.DataConsumers;
using UI.Helpers;
using UI.HttpClientServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredModal();
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<HttpClient, HttpClient>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();

builder.Services.AddTransient<TempConsumer>();
builder.Services.AddTransient<EnrollmentConsumer>();

builder.Services.AddHttpClient<ApiService>("api", c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiAddress")));


builder.Services.AddAuthorizationCore(options => new Policies(options));
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ReserverTokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ReserverTokenAuthenticationStateProvider>());
HttpClientExtenstions.tokenStorage = builder.Services.BuildServiceProvider().GetRequiredService<ReserverTokenAuthenticationStateProvider>();

await builder.Build().RunAsync();
