using MediatR;
using SaysanPwa.Domain.AggregateModels.LocationAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Location;

public class GetRegionNameByRegionIdQuery : IRequest<SysResult<string>>
{
    public int RegionId { get; set; }

    public GetRegionNameByRegionIdQuery(int regionId) => RegionId = regionId;
}

public class GetRegionNameByRegionIdQueryHandler : IRequestHandler<GetRegionNameByRegionIdQuery, SysResult<string>>
{
    private IRegionRepository _repository;

    public GetRegionNameByRegionIdQueryHandler(IRegionRepository repository) => _repository = repository;

    public async Task<SysResult<string>> Handle(GetRegionNameByRegionIdQuery request, CancellationToken cancellationToken) =>
        await _repository.GetRegionNameById(request.RegionId);
}
