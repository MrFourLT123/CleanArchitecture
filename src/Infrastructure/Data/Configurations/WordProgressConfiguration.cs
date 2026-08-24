using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class WordProgressConfiguration : IEntityTypeConfiguration<WordProgress>
{
    public void Configure(EntityTypeBuilder<WordProgress> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => new { x.UserId, x.WordId })
            .IsUnique();
    }
}
