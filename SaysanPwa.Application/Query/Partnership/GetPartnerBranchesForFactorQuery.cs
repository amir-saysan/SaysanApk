using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;

namespace SaysanPwa.Application.Query.Partnership;

public record GetPartnerBranchesForFactorQuery(long partnerId) : IRequest<IEnumerable<GetPartnerBranchesForFactorResponseDto>>;

public class GetPartnerBranchesForFactorQueryHandler : IRequestHandler<GetPartnerBranchesForFactorQuery, IEnumerable<GetPartnerBranchesForFactorResponseDto>>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IMapper _mapper;

    public GetPartnerBranchesForFactorQueryHandler(IMapper mapper, IPartnerRepository partnerRepository)
    {
        _mapper = mapper;
        _partnerRepository = partnerRepository;
    }

    public async Task<IEnumerable<GetPartnerBranchesForFactorResponseDto>> Handle(GetPartnerBranchesForFactorQuery request, CancellationToken cancellationToken)
    {
        var result = (await _partnerRepository.GetAllPartnerBranchesAsync(request.partnerId)).Result;
        return _mapper.Map<IEnumerable<GetPartnerBranchesForFactorResponseDto>>(result);
    }
}
