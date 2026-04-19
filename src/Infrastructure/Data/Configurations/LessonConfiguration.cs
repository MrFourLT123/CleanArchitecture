using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lessons>
{
    public void Configure(EntityTypeBuilder<Lessons> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.Title).IsRequired();
        builder.Property(t => t.Content).IsRequired();
    }
}
