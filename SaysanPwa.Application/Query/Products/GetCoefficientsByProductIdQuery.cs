using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Products;

public class GetCoefficientsByProductIdQuery : IRequest<SysResult<IReadOnlyCollection<ProductCoefficient>>>
{
    public long ProductId { get; set; }

    public GetCoefficientsByProductIdQuery(long productId) => ProductId = productId; 
}


public class GetCoefficientsByProductIdQueryHandler : IRequestHandler<GetCoefficientsByProductIdQuery, SysResult<IReadOnlyCollection<ProductCoefficient>>>
{
    private readonly IProductRepository _repository;

    public GetCoefficientsByProductIdQueryHandler(IProductRepository repostiory) => _repository = repostiory;

    public async Task<SysResult<IReadOnlyCollection<ProductCoefficient>>> Handle(GetCoefficientsByProductIdQuery request, CancellationToken cancellationToken) =>
        await _repository.GetCoefficientsByProductId(request.ProductId, cancellationToken);
}