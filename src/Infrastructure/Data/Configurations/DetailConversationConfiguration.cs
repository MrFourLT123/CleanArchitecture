using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DetailConversationConfiguration : IEntityTypeConfiguration<DetailConversation>
{
    public void Configure(EntityTypeBuilder<DetailConversation> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.ConversationId).IsRequired();
        builder.Property(t => t.SideA).IsRequired();
        builder.Property(t => t.SideAVN).IsRequired(false);
        builder.Property(t => t.SideB).IsRequired();
        builder.Property(t => t.SideBVN).IsRequired(false);
    }
}