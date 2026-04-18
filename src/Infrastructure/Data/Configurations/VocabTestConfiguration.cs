using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class VocabTestConfiguration: IEntityTypeConfiguration<VocabTests>
{
    public void Configure(EntityTypeBuilder<VocabTests> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.Title).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(255).IsRequired(false);
        builder.Property(t => t.CategoryId).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
    }
}
