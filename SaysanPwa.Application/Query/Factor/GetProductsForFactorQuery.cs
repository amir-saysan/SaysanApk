using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using System.Collections.Generic;

namespace SaysanPwa.Application.Query.Factor;

public record GetProductsForFactorQuery : IRequest<IEnumerable<GetProductsForFactorDto>>;

public class GetProductsForFactorQueryHandler : IRequestHandler<GetProductsForFactorQuery, IEnumerable<GetProductsForFactorDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsForFactorQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<GetProductsForFactorDto>> Handle(GetProductsForFactorQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetProductsForFactorViewModel> products = (await _productRepository.GetProductsForFactor()).Result;
        IEnumerable<GetProductsForFactorDto> result = _mapper.Map<IEnumerable<GetProductsForFactorDto>>(products);
        foreach (var product in result)
        {
            if (product.Pic_Kala is not null)
            {
                product.base64StringPicture = Convert.ToBase64String(product.Pic_Kala);
            }
        }
        return result;
    }
}

