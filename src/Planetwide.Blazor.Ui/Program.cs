global using Planetwide.Blazor.Ui.Data.Api;
global using System.Linq;
global using System.Reactive.Linq;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Planetwide.Blazor.Ui;
using StrawberryShake;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var gateway = builder.Configuration["Graphql:Gateway"];
ArgumentNullException.ThrowIfNull(gateway, "gateway");

builder.Services
    .AddPlanetwideApi(ExecutionStrategy.CacheAndNetwork)
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(gateway));    

await builder.Build().RunAsync();
