using MediatR;
using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

namespace SaysanPwa.Application.Query.ReceiptSheet;

public class GetAllCashDesksQuery : IRequest<IEnumerable<tbl_Sandog>>
{
}

public class GetAllCashDesksQueryHandler : IRequestHandler<GetAllCashDesksQuery, IEnumerable<tbl_Sandog>>
{
    private readonly IReceiptSheetRepository _repostiory;

    public GetAllCashDesksQueryHandler(IReceiptSheetRepository repository)
    {
        _repostiory = repository;
    }
    public async Task<IEnumerable<tbl_Sandog>> Handle(GetAllCashDesksQuery request, CancellationToken cancellationToken) =>
        await _repostiory.CashDesks();
}