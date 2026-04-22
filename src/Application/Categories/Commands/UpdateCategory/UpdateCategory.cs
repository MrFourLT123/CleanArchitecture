using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand: IRequest
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}

public class UpdateCategoryCommandHandler: IRequestHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FindAsync(new [] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        entity.Name = request.Name;
        entity.Description = request.Description;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
