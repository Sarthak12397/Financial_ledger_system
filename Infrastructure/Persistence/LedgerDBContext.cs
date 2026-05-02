using Microsoft.EntityFrameworkCore;

public class LedgerDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<LedgerEntry> LedgerEntries { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }

    public LedgerDbContext(DbContextOptions<LedgerDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LedgerDbContext).Assembly);
    }
}