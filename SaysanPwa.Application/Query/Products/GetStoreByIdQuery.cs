using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Products;

public class GetStoreByIdQuery : IRequest<SysResult<IReadOnlyCollection<string>>>
{
    public string Ids { get; set; } = string.Empty;

    public GetStoreByIdQuery(string ids) => Ids = ids;
}

public class GetStoreByIdQueryHandler : IRequestHandler<GetStoreByIdQuery, SysResult<IReadOnlyCollection<string>>>
{
    private readonly IProductRepository _repository;

    public GetStoreByIdQueryHandler(IProductRepository repository) => _repository = repository;

    public async Task<SysResult<IReadOnlyCollection<string>>> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken) => await _repository.GetProductStores(request.Ids);
}
