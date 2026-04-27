using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class JournalEntryConfiguration:IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable("journal_entries");
        builder.HasKey(x=> x.Id);
        builder.Property(x => x.Description)
        .IsRequired()
        .HasMaxLength(250);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.HasMany<LedgerEntry>()
        .WithOne()
        .HasForeignKey(x => x.JournalEntryId);
        builder.Property(x => x.IdempotencyKey)
    .IsRequired()
    .HasMaxLength(100);

builder.HasIndex(x => x.IdempotencyKey)
    .IsUnique();
    }

}