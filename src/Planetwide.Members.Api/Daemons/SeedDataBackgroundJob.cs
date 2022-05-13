namespace Planetwide.Members.Api.Daemons;

using Features.Members;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class SeedDataBackgroundJob : IHostedService
{
    private readonly IDbContextFactory<MemberContext> _memberContext;
    private readonly ILogger<SeedDataBackgroundJob> _logger;

    private Task _seedTask = null!;

    public SeedDataBackgroundJob(IDbContextFactory<MemberContext> memberContext, ILogger<SeedDataBackgroundJob> logger)
    {
        _memberContext = memberContext;
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
        _logger.LogInformation("Seeding members");
        await using var context = await _memberContext.CreateDbContextAsync(cancellationToken);

        for (var i = 0; i < 1000; i++)
        {
            await context.Members.AddAsync(new Member
            {
                Firstname = Faker.Name.First(),
                Surname = Faker.Name.Last()
            }, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Member data was seeded");
    }
}