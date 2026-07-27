using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Conversations.Commands.CreateConversation;

public record CreateConversationCommand : IRequest<int>
{
    public int Id { get; set; }
    public required string ConName { get; set; }
    public string? ConNameVN { get; set; }
    public string? URL { get; set; }
    public string? Character1 { get; set; }
    public string? Character2 { get; set; } = null;
}

public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateConversationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Conversation
        {
            ConName = request.ConName,
            ConNameVN = request.ConNameVN,
            URL = request.URL,
            Character1 = request.Character1,
            Character2 = request.Character2
        };
        _context.Conversations.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
