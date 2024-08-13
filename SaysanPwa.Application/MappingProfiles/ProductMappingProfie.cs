using AutoMapper;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.DTOs.Product;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;

namespace SaysanPwa.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, ProductsForSearchDto>();
        CreateMap<SmallOfGoods, SmallPurchaseOfGoods>();
        CreateMap<SmallOfGoods, RetailSaleOfGoods>();
    }
}