using CleanArchitecture.Application.DetailConversations.Commands.CreateDetailConversation;
using CleanArchitecture.Application.DetailConversations.Commands.DeleteDetailConversation;
using CleanArchitecture.Application.DetailConversations.Commands.UpdateDetailConversation;
using CleanArchitecture.Application.DetailConversations.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CleanArchitecture.Web.Endpoints;

public class DetailConversations : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/DetailConversations", GetDetailConversationList);
        groupBuilder.MapPost(CreateDetailConversation);
        groupBuilder.MapPut(UpdateDetailConversation, "{id}");
        groupBuilder.MapDelete(DeleteDetailConversation, "{id}");
    }

    [EndpointSummary("Get all DetailConversations")]
    [EndpointDescription("Retrieves all DetailConversations along with their details.")]
    public static async Task<Ok<List<DetailConversation>>> GetDetailConversationList(ISender sender)
    {
        var query = new GetDetailConversationsQuery();
        var DetailConversations = await sender.Send(query);
        return TypedResults.Ok(DetailConversations);
    }

    [EndpointSummary("Create a new DetailConversation")]
    [EndpointDescription("Creates a new DetailConversation using the provided details and returns the ID of the created DetailConversation.")]
    public static async Task<Created<int>> CreateDetailConversation(ISender sender, CreateDetailConversationCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(DetailConversations)}/{id}", id);
    }

    [EndpointSummary("Update an existing DetailConversation")]
    [EndpointDescription("Updates an existing DetailConversation using the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateDetailConversation(ISender sender, int id, UpdateDetailConversationCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a DetailConversation")]
    [EndpointDescription("Deletes a DetailConversation with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteDetailConversation(ISender sender, int id)
    {
        await sender.Send(new DeleteDetailConversationCommand(id));

        return TypedResults.NoContent();
    }
}
