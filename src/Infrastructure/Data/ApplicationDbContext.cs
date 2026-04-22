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

    public DbSet<Archievements> Archievements => Set<Archievements>();

    public DbSet<Lesson> Lessons => Set<Lesson>();

    public DbSet<Progress> Progress => Set<Progress>();

    public DbSet<VocabCards> VocabCards => Set<VocabCards>();

    public DbSet<Excercises> Excercises => Set<Excercises>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Leaderboard> Leaderboards => Set<Leaderboard>();

    public DbSet<UserAchivements> UserAchivements => Set<UserAchivements>();

    public DbSet<UserVocabTestAnswers> UserVocabTestAnswers => Set<UserVocabTestAnswers>();

    public DbSet<UserVocabTestResults> UserVocabTestResults => Set<UserVocabTestResults>();

    public DbSet<VocabTestOptions> VocabTestOptions => Set<VocabTestOptions>();

    public DbSet<VocabTests> VocabTests => Set<VocabTests>();

    public DbSet<VocabTestQuestions> VocabTestQuestions => Set<VocabTestQuestions>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
