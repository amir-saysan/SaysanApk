using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Products;

public class GetProductByIdQuery : IRequest<SysResult<Product>>
{
    public long ProductId { get; set; }

    public GetProductByIdQuery(long productId) => ProductId = productId;
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, SysResult<Product>>
{
    private readonly IProductRepository _repository;

    public GetProductByIdQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<SysResult<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken) => await _repository.GetProductById(request.ProductId, cancellationToken);
}
