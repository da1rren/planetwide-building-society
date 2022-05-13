namespace Planetwide.Members.Api.Daemons;

using Members.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class MigrationBackgroundJob : IHostedService
{
    private readonly IDbContextFactory<MemberContext> _memberContextFactory;
    private Task _backgroundTask = null!;

    public MigrationBackgroundJob(IDbContextFactory<MemberContext> memberContextFactory)
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
        await using var memberContext = await _memberContextFactory.CreateDbContextAsync(cancellationToken);
        await memberContext.Database.MigrateAsync(cancellationToken: cancellationToken);
    }
}