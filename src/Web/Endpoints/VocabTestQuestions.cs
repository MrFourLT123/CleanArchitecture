using CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;
using CleanArchitecture.Application.VocabTestQuestions.Commands.DeleteVocabTestQuestion;
using CleanArchitecture.Application.VocabTestQuestions.Commands.UpdateVocabTestQuestion;
using CleanArchitecture.Application.VocabTestQuestions.Queries.GetVocabTestQuestionList;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Web.Endpoints;

public class VocabTestQuestions : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/vocabTestQuestions", GetVocabTestQuestionList);
        groupBuilder.MapPost("/vocabTestQuestions", CreateVocabTestQuestion);
        groupBuilder.MapPut("/vocabTestQuestions/{id}", UpdateVocabTestQuestion);
        groupBuilder.MapDelete("/vocabTestQuestions/{id}", DeleteVocabTestQuestion);
    }

    [EndpointSummary("Get all VocabTestQuestions")]
    [EndpointDescription("Retrieves all VocabTestQuestions along with their details.")]
    public static async Task<Ok<List<VocabTestQuestion>>> GetVocabTestQuestionList(ISender sender)
    {
        var query = new GetVocabTestQuestionQuery();
        var vocabTestQuestions = await sender.Send(query);
        return TypedResults.Ok(vocabTestQuestions);
    }

    [EndpointSummary("Create a new VocabTestQuestion")]
    [EndpointDescription("Creates a new VocabTestQuestion.")]
    public static async Task<Created<int>> CreateVocabTestQuestion(ISender sender, CreateVocabTestQuestionCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(VocabTestQuestions)}/{id}", id);
    }

    [EndpointSummary("Update an existing VocabTestQuestion")]
    [EndpointDescription("Updates an existing VocabTestQuestion.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateVocabTestQuestion(ISender sender, int id, UpdateVocabTestQuestionCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a VocabTestQuestion")]
    [EndpointDescription("Deletes a VocabTestQuestion.")]
    public static async Task<Results<NoContent, NotFound>> DeleteVocabTestQuestion(ISender sender, int id)
    {
        await sender.Send(new DeleteVocabTestQuestionCommand(id));

        return TypedResults.NoContent();
    }
}