using MediatR;
using SaysanPwa.Domain.AggregateModels.LocationAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Location;

public class GetCityNameByCityIdQuery : IRequest<SysResult<string>>
{
    public int CityId { get; set; }


    public GetCityNameByCityIdQuery(int cityId) => CityId = cityId;
}

public class GetCityNameByCityIdQueryHandler : IRequestHandler<GetCityNameByCityIdQuery, SysResult<string>>
{
    private readonly ICityRepository _repository;

    public GetCityNameByCityIdQueryHandler(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<SysResult<string>> Handle(GetCityNameByCityIdQuery request, CancellationToken cancellationToken) =>
        await _repository.GetCityNameById(request.CityId);
}
