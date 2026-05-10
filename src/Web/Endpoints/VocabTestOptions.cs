using CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;
using CleanArchitecture.Application.VocabTestOptions.Commands.DeleteVocabTestOption;
using CleanArchitecture.Application.VocabTestOptions.Queries.GetVocabTestOption;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Application.VocabTestOptions.Commands.CreateVocabTestOption;

public class VocabTestOptions : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/vocabTestOptions", GetVocabTestOptionList);
        groupBuilder.MapPost(CreateVocabTestOption);
        groupBuilder.MapPut(UpdateVocabTestOption, "{id}");
        groupBuilder.MapDelete(DeleteVocabTestOption, "{id}");
    }

    [EndpointSummary("Get all VocabTestOptions")]
    [EndpointDescription("Retrieves all VocabTestOptions along with their details.")]
    public static async Task<Ok<List<VocabTestOption>>> GetVocabTestOptionList(ISender sender)
    {
        var query = new GetVocabTestOptionQuery();
        var vocabTestOptions = await sender.Send(query);
        return TypedResults.Ok(vocabTestOptions);
    }

    [EndpointSummary("Create a new VocabTestOption")]
    [EndpointDescription("Creates a new VocabTestOption with the provided details and returns the ID of the created VocabTestOption.")]
    public static async Task<Created<int>> CreateVocabTestOption(ISender sender, CreateVocabTestOptionCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(VocabTestOptions)}/{id}", id);
    }

    [EndpointSummary("Update an existing VocabTestOption")]
    [EndpointDescription("Updates an existing VocabTestOption with the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateVocabTestOption(ISender sender, int id, UpdateVocabTestOptionCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a VocabTestOption")]
    [EndpointDescription("Deletes a VocabTestOption with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteVocabTestOption(ISender sender, int id)
    {
        await sender.Send(new DeleteVocabTestOptionCommand(id));

        return TypedResults.NoContent();
    }
}