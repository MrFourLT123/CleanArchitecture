using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class UserAchivementConfiguration : IEntityTypeConfiguration<UserAchivement>
{
    public void Configure(EntityTypeBuilder<UserAchivement> builder)
    {
        builder.ToTable("UserAchievements");
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.UserId).IsRequired();
        builder.Property(t => t.AchievementId).IsRequired();
    }
}
