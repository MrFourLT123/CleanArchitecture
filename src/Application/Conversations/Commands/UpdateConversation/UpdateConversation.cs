using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Conversations.Commands.UpdateConversation;

public record UpdateConversationCommand : IRequest
{
    public int Id { get; set; }
    public required string ConName { get; set; }
    public string? ConNameVN { get; set; }
    public string? URL { get; set; }
    public string? Character1 { get; set; }
    public string? Character2 { get; set; } = null;
}

public class UpdateConversationCommandHandler : IRequestHandler<UpdateConversationCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateConversationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateConversationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Conversations.FindAsync(new[] { request.Id }, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        entity.ConName = request.ConName;
        entity.ConNameVN = request.ConNameVN;
        entity.URL = request.URL;
        entity.Character1 = request.Character1;
        entity.Character2 = request.Character2;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
