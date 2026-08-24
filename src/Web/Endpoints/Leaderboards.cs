using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;
using CleanArchitecture.Application.LeaderBoards.Commands.DeleteLeaderboard;
using CleanArchitecture.Application.LeaderBoards.Commands.UpdateLeaderboard;
using CleanArchitecture.Application.LeaderBoards.Queries.GetLeaderboards;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Web.Endpoints;

public class Leaderboards : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetLeaderboardsAsync);
        groupBuilder.MapPost(CreateLeaderboards);
        groupBuilder.MapPut(UpdateLeaderboards, "{userId}");
        groupBuilder.MapDelete(DeleteLeaderboards, "{id}");
    }

    [EndpointSummary("Get paginated Leaderboards")]
    [EndpointDescription("Retrieves paginated Leaderboards along with their details.")]
    public static async Task<Ok<PaginatedList<LeaderboardDto>>> GetLeaderboardsAsync(ISender sender, [AsParameters] GetLeaderboardsQuery query)
    {
        var leaderboards = await sender.Send(query);
        return TypedResults.Ok(leaderboards);
    }

    [EndpointSummary("Create a new Leaderboard")]
    [EndpointDescription("Creates a new Leaderboard with the provided details.")]
    public static async Task<Created<int>> CreateLeaderboards(ISender sender, CreateLeaderboardCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Leaderboard)}/{id}", id);
    }

    [EndpointSummary("Update an existing Leaderboard")]
    [EndpointDescription("Updates an existing Leaderboard with the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateLeaderboards(ISender sender, string userId, UpdateLeaderboardCommand command)
    {
        if (userId != command.UserId)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a Leaderboard")]
    [EndpointDescription("Deletes a Leaderboard with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteLeaderboards(ISender sender, int id)
    {
        await sender.Send(new DeleteLeaderboardCommand(id));

        return TypedResults.NoContent();
    }


}