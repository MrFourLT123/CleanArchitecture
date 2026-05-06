using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class ExcercisesConfiguration : IEntityTypeConfiguration<Excercise>
{
    public void Configure(EntityTypeBuilder<Excercise> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.LessonId).IsRequired();
        builder.Property(t => t.Question).IsRequired();
        builder.Property(t => t.Type).IsRequired();
    }
}
