using System.Text.RegularExpressions;
using Application.VocabCards.Queries.GetVocabCards;
using CleanArchitecture.Application.VocabCards.Commands.CreateVocabCard;
using CleanArchitecture.Application.VocabCards.Commands.DeleteVocabCard;
using CleanArchitecture.Application.VocabCards.Commands.UpdateVocabCard;
using CleanArchitecture.Application.VocabCards.Queries.GetVocabCards;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Web.Endpoints;

public class VocabCards : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/vocabcards", GetVocabCardList);
        groupBuilder.MapPost(CreateVocabCard);
        groupBuilder.MapPut(UpdateVocabCard, "{id}");
        groupBuilder.MapDelete(DeleteVocabCard, "{id}");
    }

    [EndpointSummary("Get all VocabCards")]
    [EndpointDescription("Retrieves all vocab cards along with their details.")]
    public static async Task<Ok<List<VocabCardDto>>> GetVocabCardList(ISender sender)
    {
        var query = new GetVocabCardQuery();
        var vocabCards = await sender.Send(query);
        return TypedResults.Ok(vocabCards);
    }

    [EndpointSummary("Update an existing VocabCard")]
    [EndpointDescription("Updates an existing vocab card with the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateVocabCard(ISender sender, int id, UpdateVocabCardCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a VocabCard")]
    [EndpointDescription("Deletes a vocab card with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteVocabCard(ISender sender, int id)
    {
        await sender.Send(new DeleteVocabCardCommand(id));
        // Implement delete logic here
        return TypedResults.NoContent();
    }

    [EndpointSummary("Create a new VocabCard")]
    [EndpointDescription("Creates a new vocab card with the provided details.")]
    public static async Task<Created<int>> CreateVocabCard(ISender sender, CreateVocabCardCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(VocabCards)}/{id}", id);
    }
}
