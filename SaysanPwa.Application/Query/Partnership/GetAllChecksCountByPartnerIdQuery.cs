using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Partnership;

public class GetAllChecksCountByPartnerIdQuery : IRequest<SysResult<int>>
{
    public GetAllChecksCountByPartnerIdQuery(rdoEnglish_V_Ch rdoEnglish_V_Ch, long fiscalYear, long partnerId, string? from, string? to)
    {
        RdoEnglish_V_Ch = rdoEnglish_V_Ch;
        FiscalYear = fiscalYear;
        PartnerId = partnerId;
        From = from;
        To = to;
    }

    public rdoEnglish_V_Ch RdoEnglish_V_Ch { get; set; }
    public long FiscalYear { get; set; }
    public long PartnerId { get; set; }
    public string? From { get; set; } = null!;
    public string? To { get; set; } = null!;
}


public class GetAllChecksCountByPartnerIdQueryHandler : IRequestHandler<GetAllChecksCountByPartnerIdQuery, SysResult<int>>
{
    private readonly IPartnerRepository _repository;

    public GetAllChecksCountByPartnerIdQueryHandler(IPartnerRepository repostiory) => _repository = repostiory;

    public async Task<SysResult<int>> Handle(GetAllChecksCountByPartnerIdQuery request, CancellationToken cancellationToken) =>
        await _repository.GetCheckCountByPartnerId(request.RdoEnglish_V_Ch, request.FiscalYear, request.PartnerId, request.From, request.To, cancellationToken);
}