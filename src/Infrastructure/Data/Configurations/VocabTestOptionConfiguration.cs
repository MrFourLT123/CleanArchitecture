using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class VocabTestOptionConfiguration : IEntityTypeConfiguration<VacabTestOptions>
{
    public void Configure(EntityTypeBuilder<VacabTestOptions> builder)
    {
        builder.Property(t=> t.Id)
            .IsRequired();
        builder.Property(t=> t.VocabTestQuestionId)
            .IsRequired();
        builder.Property(t => t.OptionLabel)
             .HasMaxLength(10)
             .IsRequired(false);
        builder.Property(t => t.OptionText)
            .HasMaxLength(200)
            .IsRequired(false);
    }
}
