using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class PhraseGroupProgressConfiguration : IEntityTypeConfiguration<PhraseGroupProgress>
{
    public void Configure(EntityTypeBuilder<PhraseGroupProgress> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.UserId).IsRequired().HasMaxLength(450);
        builder.Property(t => t.GroupId).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Status).IsRequired().HasMaxLength(20).HasDefaultValue("opened");
        builder.HasIndex(t => new { t.UserId, t.GroupId }).IsUnique();
    }
}
