using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class VocabCardsConfiguration: IEntityTypeConfiguration<VocabCards>
{
    public void Configure(EntityTypeBuilder<VocabCards> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.Word).IsRequired();
        builder.Property(t => t.Definition);
        builder.Property(t => t.ExampleSentence);
        builder.Property(t => t.ImageUrl);
        builder.Property(t => t.AudioUrl);
        builder.Property(t => t.CategoryId).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
    }
}
