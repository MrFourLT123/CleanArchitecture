using CleanArchitecture.Application.Conversations.Commands.CreateConversation;
using CleanArchitecture.Application.Conversations.Commands.DeleteConversation;
using CleanArchitecture.Application.Conversations.Commands.UpdateConversation;
using CleanArchitecture.Application.Conversations.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CleanArchitecture.Web.Endpoints;

public class Conversations : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/Conversations", GetConversationList);
        groupBuilder.MapPost(CreateConversation);
        groupBuilder.MapPut(UpdateConversation, "{id}");
        groupBuilder.MapDelete(DeleteConversation, "{id}");
    }

    [EndpointSummary("Get all Conversations")]
    [EndpointDescription("Retrieves all Conversations along with their details.")]
    public static async Task<Ok<List<Conversation>>> GetConversationList(ISender sender)
    {
        var query = new GetConversationsQuery();
        var Conversations = await sender.Send(query);
        return TypedResults.Ok(Conversations);
    }

    [EndpointSummary("Create a new Conversation")]
    [EndpointDescription("Creates a new Conversation using the provided details and returns the ID of the created Conversation.")]
    public static async Task<Created<int>> CreateConversation(ISender sender, CreateConversationCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Conversations)}/{id}", id);
    }

    [EndpointSummary("Update an existing Conversation")]
    [EndpointDescription("Updates an existing Conversation using the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateConversation(ISender sender, int id, UpdateConversationCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a Conversation")]
    [EndpointDescription("Deletes a Conversation with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteConversation(ISender sender, int id)
    {
        await sender.Send(new DeleteConversationCommand(id));

        return TypedResults.NoContent();
    }
}
