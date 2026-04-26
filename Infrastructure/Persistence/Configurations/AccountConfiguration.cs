using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
        public void Configure(EntityTypeBuilder<Account> builder)
    {
        
            builder.ToTable("accounts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(100);

            builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3);
            builder.Property(x => x.AccountType).HasConversion<string>();
            builder.Property(x => x.Balance).HasColumnName("decimal(18,4)");
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            

    }
}