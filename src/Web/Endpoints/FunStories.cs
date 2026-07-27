using CleanArchitecture.Application.FunStories.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CleanArchitecture.Web.Endpoints;

public class FunStories : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/FunStories", GetFunStoryList);
    }

    [EndpointSummary("Get all FunStories")]
    [EndpointDescription("Retrieves all FunStories along with their details.")]
    public static async Task<Ok<List<FunStory>>> GetFunStoryList(ISender sender)
    {
        var query = new GetFunStoriesQuery();
        var funStories = await sender.Send(query);
        return TypedResults.Ok(funStories);
    }
}
