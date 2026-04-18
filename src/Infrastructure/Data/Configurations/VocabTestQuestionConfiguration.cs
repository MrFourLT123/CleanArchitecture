using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class VocabTestQuestionConfiguration: IEntityTypeConfiguration<VocabTestQuestions>
{
    public void Configure(EntityTypeBuilder<VocabTestQuestions> builder)
    {
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.VocabTestId).IsRequired();
        builder.Property(t => t.VocabCardId).IsRequired();
        builder.Property(t => t.QuestionText).HasMaxLength(500);
        builder.Property(t => t.CorrectOption).HasMaxLength(100);
    }
}
