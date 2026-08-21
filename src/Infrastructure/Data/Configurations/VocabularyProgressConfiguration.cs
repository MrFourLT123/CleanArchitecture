using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class VocabularyProgressConfiguration : IEntityTypeConfiguration<VocabularyProgress>
{
    public void Configure(EntityTypeBuilder<VocabularyProgress> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.UserId).IsRequired().HasMaxLength(450);
        builder.Property(t => t.Word).HasMaxLength(200);
        builder.Property(t => t.Category).HasMaxLength(100);
        builder.Property(t => t.VocabularyStatus).IsRequired().HasMaxLength(20).HasDefaultValue("new");
        builder.HasIndex(t => new { t.UserId, t.WordId }).IsUnique();
    }
}
