using CleanArchitecture.Application.Excercises.Commands.CreateExcercise;
using CleanArchitecture.Application.Excercises.Commands.DeleteExcercise;
using CleanArchitecture.Application.Excercises.Commands.UpdateExcercise;
using CleanArchitecture.Application.Excercises.Queries.GetExcercises;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Web.Endpoints;

public class Excercises : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/excercises", GetExcercisesAsync);
        groupBuilder.MapPost(CreateExcercises);
        groupBuilder.MapPut(UpdateExcercises, "{id}");
        groupBuilder.MapDelete(DeleteExcercises, "{id}");
    }

    [EndpointSummary("Get all Excercises")]
    [EndpointDescription("Retrieves all Excercises along with their details.")]
    public static async Task<Ok<List<Excercise>>> GetExcercisesAsync(ISender sender)
    {
        var query = new GetExcercisesQuery();
        var excercises = await sender.Send(query);
        return TypedResults.Ok(excercises);
    }

    [EndpointSummary("Create a new Excercise")]
    [EndpointDescription("Creates a new Excercise with the provided details.")]
    public static async Task<Created<int>> CreateExcercises(ISender sender, CreateExcerciseCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Excercises)}/{id}", id);
    }

    [EndpointSummary("Update an existing Excercise")]
    [EndpointDescription("Updates an existing Excercise with the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateExcercises(ISender sender, int id, UpdateExcerciseCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete an Excercise")]
    [EndpointDescription("Deletes an Excercise with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteExcercises(ISender sender, int id)
    {
        await sender.Send(new DeleteExcerciseCommand(id));

        return TypedResults.NoContent();
    }
}