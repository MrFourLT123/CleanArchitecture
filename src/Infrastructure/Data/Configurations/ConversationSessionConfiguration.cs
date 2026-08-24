using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class ConversationSessionConfiguration : IEntityTypeConfiguration<ConversationSession>
{
    public void Configure(EntityTypeBuilder<ConversationSession> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.UserId).IsRequired().HasMaxLength(450);
        builder.Property(t => t.TopicId).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.UserId);
    }
}
