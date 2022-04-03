namespace Planetwide.Accounts.Api.Infrastructure.Data;

using Features.Accounts;
using Microsoft.EntityFrameworkCore;

public class AccountContext : DbContext
{
    public DbSet<Account> Account { get; set; } = null!;

    public AccountContext(DbContextOptions<AccountContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}