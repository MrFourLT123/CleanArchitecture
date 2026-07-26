
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Phrases1000s.Queries;

public record GetPhrases1000sQuery : IRequest<List<Phrases1000>>;

public class GetPhrases1000sQueryHandler : IRequestHandler<GetPhrases1000sQuery, List<Phrases1000>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPhrases1000sQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Phrases1000>> Handle(GetPhrases1000sQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var listPhrases1000 = await _context.Phrases1000.ToListAsync(cancellationToken);
            return listPhrases1000;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while retrieving Phrases1000s.", ex);
        }
    }
}
