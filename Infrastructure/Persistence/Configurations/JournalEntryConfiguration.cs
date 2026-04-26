using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class JournalEntryConfiguration:IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        
    }

}