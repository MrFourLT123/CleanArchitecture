using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand: IRequest<int>
{
    public int Id { get; set; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}

public class CreateCategoryCommandHandler: IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Category
        {
            Name = request.Name,
            Description = request.Description
        };
        _context.Categories.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
