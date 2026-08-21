using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressSyncTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversationSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TopicId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MessageCount = table.Column<int>(type: "int", nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhraseGroupProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "opened"),
                    PhrasesCount = table.Column<int>(type: "int", nullable: false),
                    PhrasesPracticed = table.Column<int>(type: "int", nullable: false),
                    BestScore = table.Column<int>(type: "int", nullable: false),
                    FirstOpenedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPracticedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseGroupProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhraseProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PhraseId = table.Column<int>(type: "int", nullable: false),
                    PhraseText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AttemptCount = table.Column<int>(type: "int", nullable: false),
                    BestScore = table.Column<int>(type: "int", nullable: false),
                    LastScore = table.Column<int>(type: "int", nullable: false),
                    IsMastered = table.Column<bool>(type: "bit", nullable: false),
                    FirstAttemptAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAttemptAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpeakingProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AttemptCount = table.Column<int>(type: "int", nullable: false),
                    BestScore = table.Column<int>(type: "int", nullable: false),
                    LastScore = table.Column<int>(type: "int", nullable: false),
                    AverageScore = table.Column<int>(type: "int", nullable: false),
                    MasteryLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "new"),
                    FirstAttemptAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAttemptAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakingProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VocabularyProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VocabularyStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "new"),
                    AttemptCount = table.Column<int>(type: "int", nullable: false),
                    CurrentWordIndex = table.Column<int>(type: "int", nullable: false),
                    KnownAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstSeenAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyProgresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationSessions_UserId",
                table: "ConversationSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhraseGroupProgresses_UserId_GroupId",
                table: "PhraseGroupProgresses",
                columns: new[] { "UserId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhraseProgresses_UserId_PhraseId",
                table: "PhraseProgresses",
                columns: new[] { "UserId", "PhraseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpeakingProgresses_UserId_WordId",
                table: "SpeakingProgresses",
                columns: new[] { "UserId", "WordId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyProgresses_UserId_WordId",
                table: "VocabularyProgresses",
                columns: new[] { "UserId", "WordId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationSessions");

            migrationBuilder.DropTable(
                name: "PhraseGroupProgresses");

            migrationBuilder.DropTable(
                name: "PhraseProgresses");

            migrationBuilder.DropTable(
                name: "SpeakingProgresses");

            migrationBuilder.DropTable(
                name: "VocabularyProgresses");
        }
    }
}
