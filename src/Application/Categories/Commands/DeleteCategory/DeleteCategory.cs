using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(int Id): IRequest;

public class DeleteCategoryCommandHandler: IRequestHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FindAsync(new [] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
