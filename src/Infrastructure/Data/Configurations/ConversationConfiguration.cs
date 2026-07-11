using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.Property(t => t.CID).IsRequired();
        builder.Property(t => t.ConName).IsRequired();
        builder.Property(t => t.ConNameVN).IsRequired(false);
        builder.Property(t => t.URL).IsRequired(false);
        builder.Property(t => t.Character1).IsRequired(false);
        builder.Property(t => t.Character2).IsRequired(false);
    }
}