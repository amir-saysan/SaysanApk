using MediatR;
using SaysanPwa.Domain.AggregateModels.LocationAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Location;

public record GetCitiesQuery(int regionId) : IRequest<IEnumerable<City>>;


public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, IEnumerable<City>>
{
    private readonly ICityRepository _repsitory;

    public GetCitiesQueryHandler(ICityRepository repository)
    {
        _repsitory = repository;
    }

    public async Task<IEnumerable<City>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        SysResult<IEnumerable<City>> result = await _repsitory.GetCitiesAsync(request.regionId);
        return result.Result;
    }
}
