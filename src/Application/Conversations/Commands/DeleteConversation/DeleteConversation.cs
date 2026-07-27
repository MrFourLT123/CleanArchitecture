using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Conversations.Commands.DeleteConversation;

public record DeleteConversationCommand(int Id) : IRequest;

public class DeleteConversationCommandHandler : IRequestHandler<DeleteConversationCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteConversationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Conversations.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Conversations.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
