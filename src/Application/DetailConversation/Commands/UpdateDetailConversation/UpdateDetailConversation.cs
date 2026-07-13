using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.DetailConversation.Commands.UpdateDetailConversation;

public record UpdateDetailConversationCommand : IRequest
{
    public int Id { get; set; }
    public int ConversationId { get; set; } = 0;
    public string SideA { get; set; } = string.Empty;
    public string SideAVN { get; set; } = string.Empty;
    public string SideB { get; set; } = string.Empty;
    public string SideBVN { get; set; } = string.Empty;
}

public class UpdateDetailConversationCommandHandler : IRequestHandler<UpdateDetailConversationCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateDetailConversationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateDetailConversationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DetailConversations.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        entity.ConversationId = request.ConversationId;
        entity.SideA = request.SideA;
        entity.SideAVN = request.SideAVN;
        entity.SideB = request.SideB;
        entity.SideBVN = request.SideBVN;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
