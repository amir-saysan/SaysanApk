using MediatR;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Factor;

public class GetCustomPricingForProductQuery : IRequest<SysResult<FactorPricing>>
{
    public long PartnerId { get; set; }
    public long ProductId { get; set; }
    public int FiscalYear { get; set; }
    public TypeCallProcedure TypeCallProcedure { get; set; }

    public GetCustomPricingForProductQuery(TypeCallProcedure typeCallProcedure, long partnerId, long productId, int fiscalYear) => 
        (TypeCallProcedure, PartnerId, ProductId, FiscalYear) =
        (typeCallProcedure, partnerId, productId, fiscalYear);
}


public class GetCustomPricingForProductQueryHandler : IRequestHandler<GetCustomPricingForProductQuery, SysResult<FactorPricing>>
{
    private readonly IFactorRepository _repository;

    public GetCustomPricingForProductQueryHandler(IFactorRepository repository)
    {
        _repository = repository;
    }


    public async Task<SysResult<FactorPricing>> Handle(GetCustomPricingForProductQuery request, CancellationToken cancellationToken) =>
        await _repository.GetCustomPriceForProduct(request.TypeCallProcedure, request.PartnerId, request.ProductId, request.FiscalYear, cancellationToken);
}
