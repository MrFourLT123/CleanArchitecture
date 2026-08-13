using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.FunStories.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Web.Endpoints;

public class FunStories : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/FunStories", GetFunStoryList);
    }

    [EndpointSummary("Get paginated FunStories")]
    [EndpointDescription("Retrieves paginated FunStories along with their details.")]
    public static async Task<Ok<PaginatedList<FunStoriesDTO>>> GetFunStoryList(ISender sender, [AsParameters] GetFunStoriesQuery query)
    {
        var funStories = await sender.Send(query);
        return TypedResults.Ok(funStories);
    }
}

