namespace Planetwide.Members.Api.Daemons;

using Features.Members;
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

        await memberContext.AddRangeAsync(new List<Member>
        {
            new()
            {
                Id = 1,
                Firstname = "Darren",
                Surname = "Maddox",
                Preferences = new MemberMarketingPreferences
                {
                    ByEmail = DateTimeOffset.Now,
                    ByOnline = DateTimeOffset.Now
                }
            },
            new()
            {
                Id = 2,
                Firstname = "James",
                Surname = "Defty",
                Preferences = new MemberMarketingPreferences
                {
                    FaceToFace = DateTimeOffset.Now,
                    BySmsMarketing = DateTimeOffset.Now
                }
            }
        }, cancellationToken);

        await memberContext.SaveChangesAsync(cancellationToken);
    }
}