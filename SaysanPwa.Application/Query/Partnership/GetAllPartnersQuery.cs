using AutoMapper;
using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;

namespace SaysanPwa.Application.Query.Partnership
{
    public record GetAllPartnersQuery : IRequest<List<PartnerDetailViewModel>>
    {
        public int? Offset { get; set; } = 0;
        public string? SearchPattern { get; set; }


        public GetAllPartnersQuery(string? searchPattern, int? offset = 0)
        {
            this.SearchPattern = searchPattern;
            this.Offset = offset;
        }
    }

    public class GetAllPartnersQueryHandler : IRequestHandler<GetAllPartnersQuery, List<PartnerDetailViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IPartnerRepository _partnerRepository;

        public GetAllPartnersQueryHandler(IPartnerRepository partnerRepository, IMapper mapper)
        {
            _partnerRepository = partnerRepository;
            _mapper = mapper;
        }

        public async Task<List<PartnerDetailViewModel>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
        {
            var partners = await _partnerRepository.GetAllAsync(request.SearchPattern, request.Offset,cancellationToken);
            return _mapper.Map<List<PartnerDetailViewModel>>(partners.Result);
        }
    }
}
