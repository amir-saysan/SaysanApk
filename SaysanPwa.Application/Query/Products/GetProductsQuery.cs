using SaysanPwa.Application.DTOs.Product;
using AutoMapper;
using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;

namespace SaysanPwa.Application.Query.Products;

public class GetProductsQuery : IRequest<List<ProductDto>>
{
    public int Offset { get; set; }

    public GetProductsQuery(int offset = 0)
    {
        Offset = offset;
    }
}

public class GetProductsQueryHanelr : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetProductsQueryHanelr(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await _repo.GetProducts(request.Offset);
        List<ProductDto> result = _mapper.Map<List<ProductDto>>(products);
        return result;
    }
}
