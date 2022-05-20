using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Planetwide.Accounts.Api.Features.Accounts;
using Planetwide.Accounts.Api.Infrastructure.Data;
using Planetwide.Members.Api.Daemons;

namespace Planetwide.Accounts.Api.Tests.Fixtures;

public class AccountApiApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // In here you would mock deps
            services.RemoveAll<IHostedService>();
        });

        builder.ConfigureTestServices(services =>
        {
            using var sp = services.BuildServiceProvider();
            var accountContextFactory = sp.GetRequiredService<IDbContextFactory<AccountContext>>();
            var context = accountContextFactory.CreateDbContext();

            const string sortCode = "070093";

            context.Database.EnsureCreated();

            context.Account.AddRangeAsync(new[]
            {
                new Account
                {
                    Id = 1,
                    Balance = 1000,
                    Iban = $"GB00ABCD{sortCode}10000000",
                    Number = "10000000",
                    SortCode = sortCode,
                    MemberId = 1
                },
                new Account
                {
                    Id = 2,
                    Balance = 1000,
                    Iban = $"GB00ABCD{sortCode}10000001",
                    Number = "10000001",
                    SortCode = sortCode,
                    MemberId = 1
                },
                new Account
                {
                    Id = 3,
                    Balance = 1000,
                    Iban = $"GB00ABCD{sortCode}10000002",
                    Number = "10000002",
                    SortCode = sortCode,
                    MemberId = 1
                }
            });

            context.SaveChanges();
        });
        
        base.ConfigureWebHost(builder);
    }
}