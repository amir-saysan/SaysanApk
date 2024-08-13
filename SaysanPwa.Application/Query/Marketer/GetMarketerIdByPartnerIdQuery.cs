using MediatR;
using SaysanPwa.Domain.AggregateModels.MarketerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Marketer;

public class GetMarketerIdByPartnerIdQuery : IRequest<SysResult<long>>
{
    public long PartnerId { get; set; }

    public GetMarketerIdByPartnerIdQuery(long partnerId)
    {
        PartnerId = partnerId;
    }
}

public class GetMarketerIdByPartnerIdQueryHandler : IRequestHandler<GetMarketerIdByPartnerIdQuery, SysResult<long>>
{
    private readonly IMarketerRepository _repository;

    public GetMarketerIdByPartnerIdQueryHandler(IMarketerRepository repository) => _repository = repository;

    public async Task<SysResult<long>> Handle(GetMarketerIdByPartnerIdQuery request, CancellationToken cancellationToken) =>
        await _repository.GetMarketerIdByPartnerIdAsync(request.PartnerId, cancellationToken);
}
