using CleanArchitecture.Application.Common.Models;
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

    [EndpointSummary("Get paginated Phrases1000s")]
    [EndpointDescription("Retrieves paginated Phrases1000s along with their details.")]
    public static async Task<Ok<PaginatedList<Phrases1000Dto>>> GetPhrases1000List(ISender sender, [AsParameters] GetPhrases1000sQuery query)
    {
        var phrases1000s = await sender.Send(query);
        return TypedResults.Ok(phrases1000s);
    }
}
