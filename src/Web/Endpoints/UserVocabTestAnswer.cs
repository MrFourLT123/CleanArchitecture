using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;
using CleanArchitecture.Application.UserVocabTestAnswers.Commands.CreateUserVocabTestAnswer;
using CleanArchitecture.Application.UserVocabTestAnswers.Commands.UpdateUserVocabTestAnswer;
using CleanArchitecture.Application.UserVocabTestAnswers.Queries.GetuserVocabTestAnswers;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Application.UserVocabTestAnswers.Endpoints;

public class UserVocabTestAnswers : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/userVocabTestAnswers", GetUserVocabTestAnswerList);
        groupBuilder.MapPost(CreateUserVocabTestAnswer);
        groupBuilder.MapPut(UpdateUserVocabTestAnswer, "{id}");
        groupBuilder.MapDelete(DeleteUserVocabTestAnswer, "{id}");
    }

    [EndpointSummary("Get all UserVocabTestAnswers")]
    [EndpointDescription("Retrieves all user vocab test answers along with their details.")]
    public static async Task<Ok<List<UserVocabTestAnswer>>> GetUserVocabTestAnswerList(ISender sender)
    {
        var query = new GetUserVocabTestAnswerQuery();
        var userVocabTestAnswers = await sender.Send(query);
        return TypedResults.Ok(userVocabTestAnswers);
    }

    [EndpointSummary("Create a new UserVocabTestAnswer")]
    [EndpointDescription("Creates a new user vocab test answer using the provided details and returns the created answer.")]
    public static async Task<Created<int>> CreateUserVocabTestAnswer(ISender sender, CreateUserVocabTestAnswerCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"{nameof(CreateUserVocabTestAnswer)}/{id}", id);
    }

    [EndpointSummary("Update an existing UserVocabTestAnswer")]
    [EndpointDescription("Updates an existing user vocab test answer with the specified ID.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateUserVocabTestAnswer(ISender sender, int id, UpdateUserVocabTestAnswerCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a UserVocabTestAnswer")]
    [EndpointDescription("Deletes a user vocab test answer with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteUserVocabTestAnswer(ISender sender, int id)
    {
        await sender.Send(new DeleteUserVocabTestAnswerCommand(id));

        return TypedResults.NoContent();
    }
}