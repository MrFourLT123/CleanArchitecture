using CleanArchitecture.Application.Progresses.Commands.CreateProgress;
using CleanArchitecture.Application.Progresses.Commands.DeleteProgress;
using CleanArchitecture.Application.Progresses.Commands.UpdateProgress;
using CleanArchitecture.Application.Progresses.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CleanArchitecture.Web.Endpoints;

public class Progresses : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/progress", GetProgressList);
        groupBuilder.MapPost(CreateProgress);
        groupBuilder.MapPut(UpdateProgress, "{id}");
        groupBuilder.MapDelete(DeleteProgress, "{id}");
    }

    [EndpointSummary("Get all Progress records")]
    [EndpointDescription("Retrieves all progress records along with their details.")]
    public static async Task<Ok<List<Progress>>> GetProgressList(ISender sender)
    {
        var query = new GetProgressListQuery();
        var progresses = await sender.Send(query);
        return TypedResults.Ok(progresses);
    }

    [EndpointSummary("Create a new Progress record")]
    [EndpointDescription("Creates a new progress record using the provided details and returns the ID of the created record.")]
    public static async Task<Created<int>> CreateProgress(ISender sender, CreateProgressCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Progresses)}/{id}", id);
    }

    [EndpointSummary("Update an existing Progress record")]
    [EndpointDescription("Updates an existing progress record using the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateProgress(ISender sender, int id, UpdateProgressCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();
        await sender.Send(command);
        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a Progress record")]
    [EndpointDescription("Deletes a progress record with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteProgress(ISender sender, int id)
    {
        await sender.Send(new DeleteProgressCommand(id));
        return TypedResults.NoContent();
    }
}
