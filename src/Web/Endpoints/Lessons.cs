using CleanArchitecture.Application.Lessons.Commands.CreateLesson;
using CleanArchitecture.Application.Lessons.Commands.DeleteLesson;
using CleanArchitecture.Application.Lessons.Commands.UpdateLesson;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
public class Lessons : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/lessons", GetLessonList);
        groupBuilder.MapPost(CreateLesson);
        groupBuilder.MapPut(UpdateLesson, "{id}");
        groupBuilder.MapDelete(DeleteLesson, "{id}");
    }

    private static async Task<Ok<List<Lesson>>> GetLessonList(ISender sender)
    {
        var query = new GetLessonsQuery();
        var lessons = await sender.Send(query);
        return TypedResults.Ok(lessons);
    }

    [EndpointSummary("Create a new Lesson")]
    [EndpointDescription("Creates a new lesson using the provided details and returns the ID of the created lesson.")]
    public static async Task<Created<int>> CreateLesson(ISender sender, CreateLessonCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Lessons)}/{id}", id);
    }

    [EndpointSummary("Update an existing Lesson")]
    [EndpointDescription("Updates an existing lesson using the provided details.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateLesson(ISender sender, int id, UpdateLessonCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a Lesson")]
    [EndpointDescription("Deletes a lesson with the specified ID.")]
    public static async Task<Results<NoContent, NotFound>> DeleteLesson(ISender sender, int id)
    {
        await sender.Send(new DeleteLessonCommand(id));

        return TypedResults.NoContent();
    }

}
