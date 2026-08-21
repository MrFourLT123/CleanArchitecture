using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.ProgressSync.Commands.BulkSyncProgress;
using CleanArchitecture.Application.ProgressSync.Commands.SaveConversationSession;
using CleanArchitecture.Application.ProgressSync.Commands.SavePhraseGroupProgress;
using CleanArchitecture.Application.ProgressSync.Commands.SavePhraseProgress;
using CleanArchitecture.Application.ProgressSync.Commands.SaveSpeakingProgress;
using CleanArchitecture.Application.ProgressSync.Commands.SaveVocabularyProgress;
using CleanArchitecture.Application.ProgressSync.Queries.GetProgressSummary;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Web.Endpoints;

public class ProgressSync : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/progress/summary/{userId}", GetProgressSummary)
            .WithName("GetProgressSummary");

        groupBuilder.MapPost("/progress/phrase-group", SavePhraseGroupProgress)
            .WithName("SavePhraseGroupProgress");

        groupBuilder.MapPost("/progress/phrase", SavePhraseProgress)
            .WithName("SavePhraseProgress");

        groupBuilder.MapPost("/progress/conversation", SaveConversationSession)
            .WithName("SaveConversationSession");

        groupBuilder.MapPost("/progress/vocabulary", SaveVocabularyProgress)
            .WithName("SaveVocabularyProgress");

        groupBuilder.MapPost("/progress/speaking", SaveSpeakingProgress)
            .WithName("SaveSpeakingProgress");

        groupBuilder.MapPut("/progress/sync", BulkSyncProgress)
            .WithName("BulkSyncProgress");
    }

    // ──────────────────────────────────────────────
    // GET /progress/summary/{userId}
    // ──────────────────────────────────────────────
    [EndpointSummary("Get progress summary")]
    [EndpointDescription("Returns an aggregated progress summary for the user across all 4 modules: phrases, conversation, vocabulary, and speaking.")]
    public static async Task<Ok<ProgressSummaryDto>> GetProgressSummary(
        ISender sender, string userId)
    {
        var result = await sender.Send(new GetProgressSummaryQuery(userId));
        return TypedResults.Ok(result);
    }

    // ──────────────────────────────────────────────
    // POST /progress/phrase-group
    // ──────────────────────────────────────────────
    [EndpointSummary("Save phrase group progress")]
    [EndpointDescription("Upserts the user's progress for a phrase group (status, best score, phrases practised count).")]
    public static async Task<Created<int>> SavePhraseGroupProgress(
        ISender sender, SavePhraseGroupProgressCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/progress/phrase-group/{id}", id);
    }

    // ──────────────────────────────────────────────
    // POST /progress/phrase
    // ──────────────────────────────────────────────
    [EndpointSummary("Save individual phrase score")]
    [EndpointDescription("Upserts pronunciation score for a single phrase. Marks mastered when score ≥ 85 on 2+ attempts.")]
    public static async Task<Created<int>> SavePhraseProgress(
        ISender sender, SavePhraseProgressCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/progress/phrase/{id}", id);
    }

    // ──────────────────────────────────────────────
    // POST /progress/conversation
    // ──────────────────────────────────────────────
    [EndpointSummary("Save conversation session")]
    [EndpointDescription("Inserts a completed conversation session record (topic, duration, message count).")]
    public static async Task<Created<int>> SaveConversationSession(
        ISender sender, SaveConversationSessionCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/progress/conversation/{id}", id);
    }

    // ──────────────────────────────────────────────
    // POST /progress/vocabulary
    // ──────────────────────────────────────────────
    [EndpointSummary("Save vocabulary word progress")]
    [EndpointDescription("Upserts the status of a vocabulary flashcard word (new/learning/known). Records the timestamp when a word first becomes 'known'.")]
    public static async Task<Created<int>> SaveVocabularyProgress(
        ISender sender, SaveVocabularyProgressCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/progress/vocabulary/{id}", id);
    }

    // ──────────────────────────────────────────────
    // POST /progress/speaking
    // ──────────────────────────────────────────────
    [EndpointSummary("Save speaking pronunciation score")]
    [EndpointDescription("Upserts a pronunciation attempt score. Maintains rolling average, best score, and mastery level (new/learning/mastered).")]
    public static async Task<Created<int>> SaveSpeakingProgress(
        ISender sender, SaveSpeakingProgressCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/progress/speaking/{id}", id);
    }

    // ──────────────────────────────────────────────
    // PUT /progress/sync
    // ──────────────────────────────────────────────
    [EndpointSummary("Bulk sync offline progress queue")]
    [EndpointDescription("Accepts a batch of offline progress events from the mobile app and processes each one. Returns the count of events processed. Use this to flush the offline queue on app foreground.")]
    public static async Task<Ok<int>> BulkSyncProgress(
        ISender sender, BulkSyncProgressCommand command)
    {
        var count = await sender.Send(command);
        return TypedResults.Ok(count);
    }
}
