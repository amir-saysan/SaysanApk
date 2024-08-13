using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Api.Logging;
using SaysanPwa.Application.Query.Products;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;

namespace SaysanPwa.Api.Pages.Products;

public class DetailModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public DetailModel(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [FromRoute]
    public long Id { get; set; }

    [BindNever]
    public Product Product { get; set; } = null!;

    [BindNever]
    public IReadOnlyCollection<ProductCoefficient> ProductCoefficients { get; set; } = null!;

    [BindNever]
    public IReadOnlyCollection<string> Stores { get; set; }


    public async Task<IActionResult> OnGetAsync()
    {
        var result = await _mediator.Send(new GetProductByIdQuery(Id));
        if (result.Succeeded)
        {
            Product = result.Result;
            var getProductCoefficientsResult = await _mediator.Send(new GetCoefficientsByProductIdQuery(Id));
            if (getProductCoefficientsResult.Succeeded)
            {
                ProductCoefficients = getProductCoefficientsResult.Result;
            }
            var getStoresByIdResult = await _mediator.Send(new GetStoreByIdQuery(Product.ID_tbl_AnbarHa));
            if (getStoresByIdResult.Succeeded)
            {
                Stores = getStoresByIdResult.Result;
            }
            return Page();
        }
        TempData["Error"] = "مشگلی در واکشی اطلاعات به وجود آمد.";
        return Redirect("/");
    }
}
