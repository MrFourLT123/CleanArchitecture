using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class UserVocabTestResultConfiguration: IEntityTypeConfiguration<UserVocabTestResults>
{
    public void Configure(EntityTypeBuilder<UserVocabTestResults> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.UserId).IsRequired();
        builder.Property(t => t.VocabTestId).IsRequired();
        builder.Property(t => t.Score).IsRequired();
        builder.Property(t => t.TakenAt).IsRequired();
    }
}
