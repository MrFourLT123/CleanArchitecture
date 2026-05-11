using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class UserVocabTestAnswerConfiguration : IEntityTypeConfiguration<UserVocabTestAnswer>
{
    public void Configure(EntityTypeBuilder<UserVocabTestAnswer> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.UserVocabTestResultId).IsRequired();
        builder.Property(t => t.VocabTestQuestionId).IsRequired();
        builder.Property(t => t.SelectedOption).HasMaxLength(500).IsRequired();
    }
}
