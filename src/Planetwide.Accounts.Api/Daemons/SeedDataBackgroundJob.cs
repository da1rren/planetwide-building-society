namespace Planetwide.Members.Api.Daemons;

using Accounts.Api.Features.Accounts;
using Accounts.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class SeedDataBackgroundJob : IHostedService
{
    private readonly IDbContextFactory<AccountContext> _accountContext;
    private readonly ILogger<SeedDataBackgroundJob> _logger;

    private Task _seedTask = null!;

    public SeedDataBackgroundJob(IDbContextFactory<AccountContext> accountContext, ILogger<SeedDataBackgroundJob> logger)
    {
        _accountContext = accountContext;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _seedTask = Seed(cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _seedTask;
    }

    private async Task Seed(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding accounts");
        await using var context = await _accountContext.CreateDbContextAsync(cancellationToken);

        for (var i = 0; i < 1000; i++)
        {
            var accountNumber = Faker.RandomNumber
                .Next(10000000, 99999999)
                .ToString("D8");

            const string sortCode = "070093";

            await context.Account.AddAsync(new Account()
            {
                Iban = $"GB00ABCD{sortCode}{accountNumber}",
                Number = accountNumber,
                SortCode = sortCode,
                MemberId = i + 1,
                Balance = Faker.RandomNumber.Next(-10_000, 10_000)
            }, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Account data was seeded");
    }
}