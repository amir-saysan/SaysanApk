using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;

namespace SaysanPwa.Application.Query.Factor;

public record GetPartnersForFactorQuery : IRequest<IEnumerable<GetPartnersForFactorDto>>;

public class GetPartnersForFactorQueryHandler : IRequestHandler<GetPartnersForFactorQuery, IEnumerable<GetPartnersForFactorDto>>
{
    private readonly IFactorRepository _factorRepository;
    private readonly IMapper _mapper;

    public GetPartnersForFactorQueryHandler(IFactorRepository factorRepository, IMapper mapper)
    {
        _factorRepository = factorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetPartnersForFactorDto>> Handle(GetPartnersForFactorQuery request, CancellationToken cancellationToken)
    {
        var result = (await _factorRepository.GetPartnerAndBranches()).Result;
        IEnumerable<GetPartnersForFactorDto> partnersAndBranches = _mapper.Map<IEnumerable<GetPartnersForFactorDto>>(result);
        return partnersAndBranches;
    }
}
