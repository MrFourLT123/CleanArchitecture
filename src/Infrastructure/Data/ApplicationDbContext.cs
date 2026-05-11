using System.Reflection;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<Achievement> Achievements => Set<Achievement>();

    public DbSet<Leaderboard> Leaderboards => Set<Leaderboard>();

    public DbSet<Lesson> Lessons => Set<Lesson>();

    public DbSet<Progress> Progresses => Set<Progress>();

    public DbSet<VocabCard> VocabCards => Set<VocabCard>();

    public DbSet<Excercise> Excercises => Set<Excercise>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<UserAchivement> UserAchivements => Set<UserAchivement>();

    public DbSet<UserVocabTestAnswer> UserVocabTestAnswers => Set<UserVocabTestAnswer>();

    public DbSet<UserVocabTestResults> UserVocabTestResults => Set<UserVocabTestResults>();

    public DbSet<VocabTestOption> VocabTestOptions => Set<VocabTestOption>();

    public DbSet<VocabTests> VocabTests => Set<VocabTests>();

    public DbSet<VocabTestQuestion> VocabTestQuestions => Set<VocabTestQuestion>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
