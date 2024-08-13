using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Products;

public class GetAllProductProfitsCountQuery : IRequest<SysResult<int>>
{
    public GetAllProductProfitsCountQuery(long fiscalYear, long productId, string? from, string? to)
    {
        FiscalYear = fiscalYear;
        ProductId = productId;
        From = from;
        To = to;
    }

    public long FiscalYear { get; set; }
    public long ProductId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
}

public class GetAllProductProfitCountRecordQueryHandler : IRequestHandler<GetAllProductProfitsCountQuery, SysResult<int>>
{
    private readonly IProductRepository _repository;

    public GetAllProductProfitCountRecordQueryHandler(IProductRepository repository) => _repository = repository;

    public async Task<SysResult<int>> Handle(GetAllProductProfitsCountQuery request, CancellationToken cancellationToken) =>
        await _repository.ProfitWithGoodsCount(request.FiscalYear, request.ProductId, request.From, request.To);
}