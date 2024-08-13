using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Partnership;

public class GetAllSellReportsQuery : IRequest<SysResult<IEnumerable<SellReport>>>
{
    public GetAllSellReportsQuery(long partnerId, long fiscalYear, long? marketerId, long offset, string? fromDate, string? toDate)
    {
        PartnerId = partnerId;
        FiscalYear = fiscalYear;
        MarketerId = marketerId;
        Offset = offset;
        FromDate = fromDate;
        ToDate = toDate;
    }

    public long PartnerId { get; set; }
    public long FiscalYear { get; set; }
    public long? MarketerId { get; set; }
    public long Offset { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
}


public class GetAllSellReportsQueryHandler : IRequestHandler<GetAllSellReportsQuery, SysResult<IEnumerable<SellReport>>>
{
    private readonly IPartnerRepository _repository;

    public GetAllSellReportsQueryHandler(IPartnerRepository repostiory)
    {
        _repository = repostiory;
    }


    public async Task<SysResult<IEnumerable<SellReport>>> Handle(GetAllSellReportsQuery request, CancellationToken cancellationToken) =>
        await _repository.GetAllSellReportsByPartnerIdAsync(request.PartnerId, request.FiscalYear, request.MarketerId, request.Offset,
            request.FromDate, request.ToDate, cancellationToken);
}