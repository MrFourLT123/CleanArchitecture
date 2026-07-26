using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class Phrases1000Configuration : IEntityTypeConfiguration<Phrases1000>
{
    public void Configure(EntityTypeBuilder<Phrases1000> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.Phrases).IsRequired();
        builder.Property(t => t.Meaning).IsRequired();
        builder.Property(t => t.GroupId).IsRequired();
        builder.Property(t => t.Level).IsRequired();
    }
}
