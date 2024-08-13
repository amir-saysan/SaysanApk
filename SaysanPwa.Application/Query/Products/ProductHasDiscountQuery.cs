using MediatR;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using System.Globalization;

namespace SaysanPwa.Application.Query.Products;

public record ProductHasDiscountQuery(long productId, DiscountType discountType) : IRequest<HasDiscountViewModel>;


public enum DiscountType
{
    tbl_Khedmat,
    tbl_Kala
}

public class ProductHasDiscountQueryHandler : IRequestHandler<ProductHasDiscountQuery, HasDiscountViewModel>
{
    private readonly IProductRepository _productRepository;

    public ProductHasDiscountQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<HasDiscountViewModel> Handle(ProductHasDiscountQuery request, CancellationToken cancellationToken)
    {
        PersianCalendar pc = new PersianCalendar();
        DateTime now = DateTime.Now;
        string shamsiDate = $"{pc.GetYear(now)}/{pc.GetMonth(now).ToString("D2")}/{pc.GetDayOfMonth(now).ToString("D2")}";

        var result = (await _productRepository.ProductHasDiscout(request.discountType.ToString(), request.productId, shamsiDate)).Result;
        return result;
    }
}
