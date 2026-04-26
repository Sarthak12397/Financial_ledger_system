using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LedgerConfiguration:  IEntityTypeConfiguration<LedgerEntry>
{
            public void Configure(EntityTypeBuilder<LedgerEntry> builder)
    {
        builder.ToTable("ledger_entries");
        builder.HasKey(x => x.Id);

        builder.HasOne<Account>()
    .WithMany()
    .HasForeignKey(x => x.AccountId)
    .IsRequired();

    builder.HasOne<JournalEntry>()
    .WithMany()
    .HasForeignKey(x => x.JournalEntryId)
    .IsRequired();
    
    builder.Property(x => x.Description)
                    .IsRequired()
                 .HasMaxLength(250);

    builder.Property(x => x.CreatedAt).IsRequired();
    builder.Property(x => x.Amount).HasColumnType("decimal(18,4)");
        builder.Property(x => x.Entrytype).HasConversion<string>();
    }

}