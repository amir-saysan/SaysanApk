using MediatR;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Partnership;

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
    public Task<SysResult<long>> Handle(GetMarketerIdByPartnerIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
