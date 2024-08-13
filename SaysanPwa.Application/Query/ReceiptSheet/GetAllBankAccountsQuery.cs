using MediatR;
using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

namespace SaysanPwa.Application.Query.ReceiptSheet;

public class GetAllBankAccountsQuery : IRequest<IEnumerable<tbl_Hesab>>
{
}

public class GetAllBankAccountQueryHandler : IRequestHandler<GetAllBankAccountsQuery, IEnumerable<tbl_Hesab>>
{
    private IReceiptSheetRepository _repository;

    public GetAllBankAccountQueryHandler(IReceiptSheetRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<tbl_Hesab>> Handle(GetAllBankAccountsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.BankAccounts(cancellationToken);
    }
}
