using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.LeaderBoards.Commands.CreateLeaderboard;

public record UpdateVocabTestOptionCommand : IRequest
{
    public int Id { get; set; }
    public int VocabTestQuestionId { get; set; }
    public string? OptionLabel { get; set; }
    public string? OptionText { get; set; }
}

public class UpdateVocabTestOptionCommandHandler : IRequestHandler<UpdateVocabTestOptionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateVocabTestOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateVocabTestOptionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.VocabTestOptions.FindAsync(new[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);
        entity.VocabTestQuestionId = request.VocabTestQuestionId;
        entity.OptionLabel = request.OptionLabel;
        entity.OptionText = request.OptionText;

        await _context.SaveChangesAsync(cancellationToken);
    }
}