using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.DetailConversations.Commands.CreateDetailConversation;

public record CreateDetailConversationCommand : IRequest<int>
{
    public int Id { get; set; }
    public int ConversationId { get; set; } = 0;
    public string SideA { get; set; } = string.Empty;
    public string SideAVN { get; set; } = string.Empty;
    public string SideB { get; set; } = string.Empty;
    public string SideBVN { get; set; } = string.Empty;
}

public class CreateDetailConversationCommandHandler : IRequestHandler<CreateDetailConversationCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateDetailConversationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateDetailConversationCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.DetailConversation
        {
            ConversationId = request.ConversationId,
            SideA = request.SideA,
            SideAVN = request.SideAVN,
            SideB = request.SideB,
            SideBVN = request.SideBVN
        };
        _context.DetailConversations.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
