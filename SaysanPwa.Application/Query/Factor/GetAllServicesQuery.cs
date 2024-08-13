using MediatR;
using SaysanPwa.Domain.AggregateModels.Factor;
using Factor = SaysanPwa.Domain.AggregateModels.Factor;

namespace SaysanPwa.Application.Query.Factor;

public class GetAllServicesQuery : IRequest<IEnumerable<Factor::Services>>
{
}


public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, IEnumerable<Factor::Services>>
{
    private readonly IFactorRepository _factorRepository;

    public GetAllServicesQueryHandler(IFactorRepository factorRepository) => _factorRepository = factorRepository;


    public async Task<IEnumerable<Domain.AggregateModels.Factor.Services>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken) =>
        await _factorRepository.GetAllServices(cancellationToken);
}