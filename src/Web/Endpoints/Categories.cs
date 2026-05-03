
using CleanArchitecture.Application.Categories.Commands.CreateCategory;
using CleanArchitecture.Application.Categories.Commands.DeleteCategory;
using CleanArchitecture.Application.Categories.Commands.UpdateCategory;
using CleanArchitecture.Application.Categories.Queries.GetCategories;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CleanArchitecture.Web.Endpoints;

public class Categories : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateCategory);
        groupBuilder.MapGet("/categories", GetCategories);
        groupBuilder.MapPut(UpdateCategory, "{id}");
        groupBuilder.MapDelete(DeleteCategory, "{id}");
    }

    [EndpointSummary("Get all Categories")]
    [EndpointDescription("Retrieves all categories along with their details.")]
    public static async Task<Ok<List<Category>>> GetCategories(ISender sender)
    {
        var query = new GetCategoriesQuery();
        var categories = await sender.Send(query);
        return TypedResults.Ok(categories);
    }
    [EndpointSummary("Create new Category")]
    [EndpointDescription("Create new category")]
    public static async Task<Created<int>> CreateCategory(ISender sender, CreateCategoryCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"{nameof(CreateCategory)}/{id}", id);
    }
    [EndpointSummary("Update an existing Category")]
    [EndpointDescription("Updates an existing category using the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateCategory(ISender sender, int id, UpdateCategoryCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a Category")]
    [EndpointDescription("Deletes a category with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteCategory(ISender sender, int id)
    {
        await sender.Send(new DeleteCategoryCommand(id));

        return TypedResults.NoContent();
    }
}