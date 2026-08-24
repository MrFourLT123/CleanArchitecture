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

    DbSet<Phrases1000> Phrases1000 { get; }
    DbSet<PhrasesGroup> PhrasesGroup { get; }
    DbSet<FunStory> FunStories { get; }

    DbSet<Conversation> Conversations { get; }
    DbSet<DetailConversation> DetailConversations { get; }

    // Progress tracking
    DbSet<PhraseGroupProgress> PhraseGroupProgresses { get; }
    DbSet<PhraseProgress> PhraseProgresses { get; }
    DbSet<ConversationSession> ConversationSessions { get; }
    DbSet<VocabularyProgress> VocabularyProgresses { get; }
    DbSet<SpeakingProgress> SpeakingProgresses { get; }
    DbSet<WordProgress> WordProgresses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
