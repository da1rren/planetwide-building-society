using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Planetwide.Accounts.Api.Tests.Fixtures;

public class AccountApiApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // In here you would mock deps
        });
        
        base.ConfigureWebHost(builder);
    }
}