using MediatR;
using SaysanPwa.Domain.AggregateModels.LocationAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Location;

public record GetRegionsQuery : IRequest<IEnumerable<Region>>;

public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, IEnumerable<Region>>
{
    private readonly IRegionRepository _regionRepository;

    public GetRegionsQueryHandler(IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    public async Task<IEnumerable<Region>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
    {
        SysResult<IEnumerable<Region>> result = await _regionRepository.GetRegionsAsync();
        return result.Result;
    }
}
