using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.UserAchivements.Commands.DeleteUserAchivement;

public record DeleteUserAchivementCommand(int Id) : IRequest;

public class DeleteUserAchivementCommandHandler : IRequestHandler<DeleteUserAchivementCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteUserAchivementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteUserAchivementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserAchivements.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        _context.UserAchivements.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
