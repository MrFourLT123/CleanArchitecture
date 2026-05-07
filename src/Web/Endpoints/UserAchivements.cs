using CleanArchitecture.Application.UserAchivements.Commands.CreateUserAchivement;
using CleanArchitecture.Application.UserAchivements.Commands.DeleteUserAchivement;
using CleanArchitecture.Application.UserAchivements.Commands.UpdateUserAchivement;
using CleanArchitecture.Application.UserAchivements.Queries.GetuserAchivement;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Web.Endpoints;

public class UserAchivements : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/userachivements", GetUserAchivements);
        groupBuilder.MapPut(UpdateUserAchivement, "{id}");
        groupBuilder.MapPost(CreateUserAchivement);
        groupBuilder.MapDelete(DeleteUserAchivement, "{id}");

    }

    [EndpointSummary("Get all User Achivements")]
    [EndpointDescription("Retrieves all user achivements along with their details.")]
    public static async Task<Ok<List<UserAchivement>>> GetUserAchivements(ISender sender)
    {
        var query = new GetUserAchivementQuery();
        var userAchivements = await sender.Send(query);
        return TypedResults.Ok(userAchivements);
    }

    [EndpointSummary("Update an existing User Achivement")]
    [EndpointDescription("Updates an existing user achivement with the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateUserAchivement(ISender sender, int id, UpdateUserAchivementCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Create a new User Achivement")]
    [EndpointDescription("Creates a new user achivement using the provided details and returns the ID")]
    public static async Task<Created<int>> CreateUserAchivement(ISender sender, CreateUserAchivementCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(UserAchivements)}/{id}", id);
    }

    [EndpointSummary("Delete a User Achivement")]
    [EndpointDescription("Deletes an existing user achivement by its ID")]
    public static async Task<Results<NoContent, NotFound>> DeleteUserAchivement(ISender sender, int id)
    {
        await sender.Send(new DeleteUserAchivementCommand(id));

        return TypedResults.NoContent();
    }
}
