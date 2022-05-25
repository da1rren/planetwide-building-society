namespace Planetwide.Members.Api.Infrastructure.Data;

using Features.Members;
using Microsoft.EntityFrameworkCore;

public class MemberContext : DbContext
{
    public DbSet<Member> Members { get; set; } = null!;

    public MemberContext(DbContextOptions<MemberContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemberContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}