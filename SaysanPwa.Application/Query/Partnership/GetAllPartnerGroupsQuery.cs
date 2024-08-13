using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;


namespace SaysanPwa.Application.Query.Partnership;

public record GetAllPartnerGroupsQuery : IRequest<IEnumerable<GetAllPartnerGroupsResponseDto>>;

public class GetAllJobGroupsQueryHandler :
    IRequestHandler<GetAllPartnerGroupsQuery, IEnumerable<GetAllPartnerGroupsResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IPartnerRepository _partnerRepository;

    public GetAllJobGroupsQueryHandler(IPartnerRepository partnerRepository, IMapper mapper)
    {
        _partnerRepository = partnerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllPartnerGroupsResponseDto>> Handle(GetAllPartnerGroupsQuery request, CancellationToken cancellationToken)
    {
        var sysRes = await _partnerRepository.GetAllPartnerGroups(cancellationToken);
        IEnumerable<PartnerGroup> jobs = sysRes.Result;
        return _mapper.Map<List<GetAllPartnerGroupsResponseDto>>(jobs);
    }
}
