using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Partnership;

public class GetAllPartnerConsolidationOfSalesOfGoodsToCustomersQuery : IRequest<SysResult<IEnumerable<PartnerConsolidationOfSalesOfGoodsToCustomers>>>
{
    public GetAllPartnerConsolidationOfSalesOfGoodsToCustomersQuery(long partnerId, long fiscalYear, long? marketerId, long offset, string? fromDate, string? toDate)
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

public class GetAllPartnerConsolidationOfSalesOfGoodsToCustomersQueryHandler :
    IRequestHandler<GetAllPartnerConsolidationOfSalesOfGoodsToCustomersQuery, SysResult<IEnumerable<PartnerConsolidationOfSalesOfGoodsToCustomers>>>
{

    private readonly IPartnerRepository _repository;

    public GetAllPartnerConsolidationOfSalesOfGoodsToCustomersQueryHandler(IPartnerRepository repostiory)
    {
        _repository = repostiory;
    }

    public async Task<SysResult<IEnumerable<PartnerConsolidationOfSalesOfGoodsToCustomers>>> Handle(GetAllPartnerConsolidationOfSalesOfGoodsToCustomersQuery request, CancellationToken cancellationToken)
         => await _repository.GetAllPartnerConsolidationOfSalesOfGoodsToCustomers(request.PartnerId, request.FiscalYear, request.MarketerId, request.Offset, request.FromDate,
             request.ToDate, cancellationToken);
}