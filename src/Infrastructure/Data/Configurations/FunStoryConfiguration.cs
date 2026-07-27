using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class FunStoryConfiguration : IEntityTypeConfiguration<FunStory>
{
    public void Configure(EntityTypeBuilder<FunStory> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.TitleEN).IsRequired();
        builder.Property(t => t.TitleVN).IsRequired();
        builder.Property(t => t.ContentEN).IsRequired();
        builder.Property(t => t.ContentVN).IsRequired();
    }
}
