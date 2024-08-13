using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Query.Products;
using SaysanPwa.Application.Utilities.Typography;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Api.Pages.Products;

public class RetailSaleOfGoodsModel : PageModel
{
    private readonly IMediator _mediator;

    public RetailSaleOfGoodsModel(IMediator mediator) => _mediator = mediator;

    [FromRoute]
    public int ProductId { get; set; }
    [FromRoute]
    public string ProductName { get; set; } = string.Empty;

    [FromQuery]
    public int CurrentPage { get; set; } = 0;

    [FromQuery]
    public string? From { get; set; }

    [FromQuery]
    public string? To { get; set; }

    [FromQuery]
    public int? FiscalYear { get; set; }

    public PageResult<IEnumerable<SmallOfGoods>> PageResult { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var itemCountPageResult = await _mediator.Send(new GetProductCountQuery(ProcedureCallType.Riz_Forosh_Kala_Count_Record, FiscalYear == null ? 1 : FiscalYear.Value,
            ProductId, From == null ? null! : From, To == null ? null! : To));
        if (itemCountPageResult.Succeeded)
        {
            PageResult.PageInfo = new(CurrentPage, 50, itemCountPageResult.Result);
            var fetchResult = await _mediator.Send(new GetAllSmallOfGoodsQuery(SmallOfGoodsProcedureCallType.Riz_Forosh_Kala, FiscalYear == null ? 1 : FiscalYear.Value, ProductId,
                From == null ? null! : From.ToConventialText(), To == null ? null! : To.ToConventialText(), CurrentPage * 50));
            if (fetchResult.Succeeded)
            {
                PageResult.Result = fetchResult.Result;
                return Page();
            }
        }
        return Redirect("/");
    }
}
