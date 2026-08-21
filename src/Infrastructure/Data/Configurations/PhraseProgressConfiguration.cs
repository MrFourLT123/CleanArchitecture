using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class PhraseProgressConfiguration : IEntityTypeConfiguration<PhraseProgress>
{
    public void Configure(EntityTypeBuilder<PhraseProgress> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.UserId).IsRequired().HasMaxLength(450);
        builder.Property(t => t.GroupId).IsRequired().HasMaxLength(100);
        builder.Property(t => t.PhraseText).HasMaxLength(500);
        builder.HasIndex(t => new { t.UserId, t.PhraseId }).IsUnique();
    }
}
