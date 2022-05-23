using Microsoft.EntityFrameworkCore;
using Planetwide.Accounts.Api.Infrastructure.Data;

namespace Planetwide.Accounts.Api.Daemons;

using Features.Accounts;

public class MigrationBackgroundJob : IHostedService
{
    private readonly IDbContextFactory<AccountContext> _memberContextFactory;
    private Task _backgroundTask = null!;

    public MigrationBackgroundJob(IDbContextFactory<AccountContext> memberContextFactory)
    {
        _memberContextFactory = memberContextFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _backgroundTask = MigrateDatabase(cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _backgroundTask;
    }

    private async Task MigrateDatabase(CancellationToken cancellationToken)
    {
        await using var accountContext = await _memberContextFactory.CreateDbContextAsync(cancellationToken);
        await accountContext.Database.MigrateAsync(cancellationToken: cancellationToken);

        await accountContext.AddRangeAsync(new List<Account>
        {
            new()
            {
                Id = 1,
                MemberId = 1,
                Balance = 502,
                Iban = "GB38BARC20031868458639",
                Number = "10203040",
                SortCode = "070093"
            },
            new()
            {
                Id = 2,
                MemberId = 1,
                Balance = 50_000,
                Iban = "GB35BARC20035386314237",
                Number = "14233241",
                SortCode = "070093"
            },
        }, cancellationToken);

        await accountContext.SaveChangesAsync(cancellationToken);
    }
}