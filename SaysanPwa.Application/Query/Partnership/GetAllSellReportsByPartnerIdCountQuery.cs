using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Partnership;

public class GetAllSellReportsByPartnerIdCountQuery : IRequest<SysResult<int>>
{
    public GetAllSellReportsByPartnerIdCountQuery(long partnerId, long fiscalYear, long? marketerId, string? fromDate, string? toDate)
    {
        PartnerId = partnerId;
        FiscalYear = fiscalYear;
        MarketerId = marketerId;
        FromDate = fromDate;
        ToDate = toDate;
    }

    public long PartnerId { get; set; }
    public long FiscalYear { get; set; }
    public long? MarketerId { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
}

public class GetAllSellReportsByPartnerIdCountQueryHandler : IRequestHandler<GetAllSellReportsByPartnerIdCountQuery, SysResult<int>>
{
    private readonly IPartnerRepository _repository;

    public GetAllSellReportsByPartnerIdCountQueryHandler(IPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<SysResult<int>> Handle(GetAllSellReportsByPartnerIdCountQuery request, CancellationToken cancellationToken) => 
        await _repository.GetAllSellReportsByPartnerIdCountAsync(request.PartnerId, request.FiscalYear, request.MarketerId, request.FromDate, request.ToDate, cancellationToken);
}
