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
    DbSet<VocabTestQuestion> VocabTestQuestions { get; }

    DbSet<VocabTestOption> VocabTestOptions { get; }
    DbSet<UserVocabTestAnswer> UserVocabTestAnswers { get; }
    DbSet<UserVocabTestResult> UserVocabTestResults { get; }
    DbSet<VocabTests> VocabTests { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
