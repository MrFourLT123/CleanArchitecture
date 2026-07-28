using CleanArchitecture.Application.PhrasesGroups.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CleanArchitecture.Web.Endpoints;

public class PhrasesGroups : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/PhrasesGroup", GetPhrasesList);
    }

    [EndpointSummary("Get all PhrasesGroup")]
    [EndpointDescription("Retrieves all PhrasesGroup along with their details.")]
    public static async Task<Ok<List<PhrasesGroup>>> GetPhrasesList(ISender sender)
    {
        var query = new GetPhrasesGroupQuery();
        var PhrasesGroup = await sender.Send(query);
        return TypedResults.Ok(PhrasesGroup);
    }
}
