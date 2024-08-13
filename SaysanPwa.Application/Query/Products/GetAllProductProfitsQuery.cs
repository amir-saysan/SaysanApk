using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Products;

public class GetAllProductProfitsQuery : IRequest<SysResult<IEnumerable<ProductProfit>>>
{
    public GetAllProductProfitsQuery(long fiscalYear, long productId, string? from, string? to, long offset)
    {
        FiscalYear = fiscalYear;
        ProductId = productId;
        From = from;
        To = to;
        Offset = offset;
    }

    public long FiscalYear { get; set; }
    public long ProductId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public long Offset { get; set; }
}


public class GetAllProductProfitsQueryHandler : IRequestHandler<GetAllProductProfitsQuery, SysResult<IEnumerable<ProductProfit>>>
{
    private readonly IProductRepository _repository;

    public GetAllProductProfitsQueryHandler(IProductRepository repository) => _repository = repository;



    public async Task<SysResult<IEnumerable<ProductProfit>>> Handle(GetAllProductProfitsQuery request, CancellationToken cancellationToken) =>
        await _repository.GetAllProductProfits(request.FiscalYear, request.ProductId, request.From, request.To, request.Offset);
}
