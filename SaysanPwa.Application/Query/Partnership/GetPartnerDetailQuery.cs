using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;

namespace SaysanPwa.Application.Query.Partnership;

public record GetPartnerDetailQuery(long id) : IRequest<PartnerDetailDto>;

public class GetPartnerDetailQueryHandler : IRequestHandler<GetPartnerDetailQuery, PartnerDetailDto>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IMapper _mapper;

    public GetPartnerDetailQueryHandler(IMapper mapper, IPartnerRepository partnerRepository)
    {
        _mapper = mapper;
        _partnerRepository = partnerRepository;
    }

    public async Task<PartnerDetailDto> Handle(GetPartnerDetailQuery request, CancellationToken cancellationToken)
    {
        PartnerDetailViewModel partner = (await _partnerRepository.GetPartnerDetailAsync(request.id)).Result;
        PartnerDetailDto result = _mapper.Map<PartnerDetailDto>(partner);
        result.Branches = partner.Branches;
        return result;
    }
}
