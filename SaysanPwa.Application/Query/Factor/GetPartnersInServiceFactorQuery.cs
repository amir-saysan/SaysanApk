using MediatR;
using SaysanPwa.Domain.AggregateModels.Factor;

namespace SaysanPwa.Application.Query.Factor;

public class GetPartnersInServiceFactorQuery : IRequest<IEnumerable<PartnerInServiceFactor>>
{
}

public class GetPartnersInServiceFactorQueryHandler : IRequestHandler<GetPartnersInServiceFactorQuery, IEnumerable<PartnerInServiceFactor>>
{
    private readonly IFactorRepository _repository;

    public GetPartnersInServiceFactorQueryHandler(IFactorRepository repository) => _repository = repository;

    public async Task<IEnumerable<PartnerInServiceFactor>> Handle(GetPartnersInServiceFactorQuery request, CancellationToken cancellationToken) =>
        await _repository.GetAllServicePartnetsAsync(cancellationToken);
}