using CleanArchitecture.Application.UserVocabTestResults.Commands.CreateUserVocabTestResult;
using CleanArchitecture.Application.UserVocabTestResults.Commands.DeleteUserVocabTestResult;
using CleanArchitecture.Application.UserVocabTestResults.Queries.GetUserVocabTestResults;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Web.Endpoints;

public class UserVocabTestResults : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/userVocabTestResults", GetUserVocabTestResultList);
        groupBuilder.MapPost(CreateUserVocabTestResult);
        groupBuilder.MapPut(UpdateUserVocabTestResult, "{id}");
        groupBuilder.MapDelete(DeleteUserVocabTestResult, "{id}");
    }

    [EndpointSummary("Get all UserVocabTestResults")]
    [EndpointDescription("Retrieves all user vocab test results along with their details.")]

    public static async Task<Ok<List<UserVocabTestResult>>> GetUserVocabTestResultList(ISender sender)
    {
        var query = new GetUserVocabTestResultQuery();
        var userVocabTestResults = await sender.Send(query);
        return TypedResults.Ok(userVocabTestResults);
    }

    [EndpointSummary("Create a new UserVocabTestResult")]
    [EndpointDescription("Creates a new user vocab test result using the provided details and returns the created result.")]
    public static async Task<Created<int>> CreateUserVocabTestResult(ISender sender, CreateUserVocabTestResultCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"{nameof(CreateUserVocabTestResult)}/{id}", id);
    }

    [EndpointSummary("Update an existing UserVocabTestResult")]
    [EndpointDescription("Updates an existing user vocab test result with the specified ID.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateUserVocabTestResult(ISender sender, int id, UpdateUserVocabTestResultCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a UserVocabTestResult")]
    [EndpointDescription("Deletes a user vocab test result with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteUserVocabTestResult(ISender sender, int id)
    {
        await sender.Send(new DeleteUserVocabTestResultCommand(id));
        return TypedResults.NoContent();
    }
}