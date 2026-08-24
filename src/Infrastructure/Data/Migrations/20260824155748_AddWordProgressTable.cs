using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWordProgressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "WordProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VocabularyStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MasteryLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BestPronunciationScore = table.Column<int>(type: "int", nullable: false),
                    LastScore = table.Column<int>(type: "int", nullable: false),
                    AverageScore = table.Column<int>(type: "int", nullable: false),
                    PronunciationAttempts = table.Column<int>(type: "int", nullable: false),
                    VocabularyAttempts = table.Column<int>(type: "int", nullable: false),
                    TotalAttempts = table.Column<int>(type: "int", nullable: false),
                    FirstAttemptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPracticedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KnownAtDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordProgresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordProgresses_UserId_WordId",
                table: "WordProgresses",
                columns: new[] { "UserId", "WordId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordProgresses");


        }
    }
}
