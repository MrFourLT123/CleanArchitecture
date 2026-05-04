using CleanArchitecture.Application.Achievements.Commands.CreateAchievement;
using CleanArchitecture.Application.Achievements.Commands.DeleteAchievement;
using CleanArchitecture.Application.Achievements.Commands.UpdateAchievement;
using CleanArchitecture.Application.Achievements.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using MediatR;

namespace CleanArchitecture.Web.Endpoints;

public class Achievements : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/Achievements", GetAchievementList);
        groupBuilder.MapPost(CreateAchievement);
        groupBuilder.MapPut(UpdateAchievement, "{id}");
        groupBuilder.MapDelete(DeleteAchievement, "{id}");
    }

    [EndpointSummary("Get all Achievements")]
    [EndpointDescription("Retrieves all Achievements along with their details.")]
    public static async Task<Ok<List<Achievement>>> GetAchievementList(ISender sender)
    {
        var query = new GetAchievementsQuery();
        var achievements = await sender.Send(query);
        return TypedResults.Ok(achievements);
    }

    [EndpointSummary("Create a new Achievement")]
    [EndpointDescription("Creates a new Achievement using the provided details and returns the ID of the created Achievement.")]
    public static async Task<Created<int>> CreateAchievement(ISender sender, CreateAchievementCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Achievement)}/{id}", id);
    }

    [EndpointSummary("Update an existing Achievement")]
    [EndpointDescription("Updates an existing Achievement using the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateAchievement(ISender sender, int id, UpdateAchievementCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();
        await sender.Send(command);
        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete an Achievement")]
    [EndpointDescription("Deletes an Achievement with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteAchievement(ISender sender, int id)
    {
        await sender.Send(new DeleteAchievementCommand(id));
        return TypedResults.NoContent();
    }
}
