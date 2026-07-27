using CleanArchitecture.Application.Phrases1000s.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CleanArchitecture.Web.Endpoints;

public class Phrases1000s : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/Phrases1000s", GetPhrases1000List);
    }

    [EndpointSummary("Get all Phrases1000s")]
    [EndpointDescription("Retrieves all Phrases1000s along with their details.")]
    public static async Task<Ok<List<Phrases1000>>> GetPhrases1000List(ISender sender)
    {
        var query = new GetPhrases1000sQuery();
        var Phrases1000s = await sender.Send(query);
        return TypedResults.Ok(Phrases1000s);
    }
}
