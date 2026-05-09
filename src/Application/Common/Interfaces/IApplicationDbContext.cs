using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<Category> Categories { get; }
    DbSet<Lesson> Lessons { get; }
    DbSet<Achievement> Achievements { get; }
    DbSet<Leaderboard> Leaderboards { get; }
    DbSet<Excercise> Excercises { get; }
    DbSet<Progress> Progresses { get; }
    DbSet<UserAchivement> UserAchivements { get; }
    DbSet<VocabCard> VocabCards { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
