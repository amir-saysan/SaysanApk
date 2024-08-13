using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;

namespace SaysanPwa.Application.Query.Products;

public class GetAllProductsCountQuery : IRequest<int>
{
    
}

public class GetAllProductsCountQueryHandler : IRequestHandler<GetAllProductsCountQuery, int>
{
    private readonly IProductRepository _repository;

    public GetAllProductsCountQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }
    public async Task<int> Handle(GetAllProductsCountQuery request, CancellationToken cancellationToken) => await _repository.GetAllProductsCount(cancellationToken);
}
