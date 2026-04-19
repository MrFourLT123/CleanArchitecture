using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class ProgressConfiguration : IEntityTypeConfiguration<CleanArchitecture.Domain.Entities.Progress>
{
    public void Configure(EntityTypeBuilder<Progress> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t =>t.UserId).IsRequired();
        builder.Property(t => t.LessonId).IsRequired();
    }
}
