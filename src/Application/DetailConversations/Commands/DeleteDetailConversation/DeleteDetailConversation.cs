using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.DetailConversations.Commands.DeleteDetailConversation;

public record DeleteDetailConversationCommand(int Id) : IRequest;

public class DeleteDetailConversationCommandHandler : IRequestHandler<DeleteDetailConversationCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteDetailConversationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteDetailConversationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DetailConversations.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.DetailConversations.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
