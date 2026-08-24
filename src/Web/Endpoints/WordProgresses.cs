using CleanArchitecture.Application.WordProgresses.Commands.SyncWordProgress;
using CleanArchitecture.Application.WordProgresses.Queries.GetWordProgress;
using Microsoft.AspNetCore.Http.HttpResults;
using MediatR;

namespace CleanArchitecture.Web.Endpoints;

public class WordProgresses : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost("/api/WordProgress/Sync", SyncWordProgress);
        groupBuilder.MapGet("/api/WordProgress", GetWordProgress);
    }

    [EndpointSummary("Sync word progress")]
    [EndpointDescription("Upserts the user's progress for a specific word, tracking both pronunciation and vocabulary stats.")]
    public static Task<int> SyncWordProgress(ISender sender, SyncWordProgressCommand command)
    {
        return sender.Send(command);
    }

    [EndpointSummary("Get user word progress")]
    [EndpointDescription("Retrieves all synced word progress for a given user.")]
    public static Task<List<WordProgressDto>> GetWordProgress(ISender sender, [AsParameters] GetWordProgressQuery query)
    {
        return sender.Send(query);
    }
}
