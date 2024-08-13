using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Query.Products;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.Products;

public class ProfitWithGoodsModel : PageModel
{
    private readonly IMediator _mediator;

    public ProfitWithGoodsModel(IMediator mediator) => _mediator = mediator;

    [FromRoute]
    public long ProductId { get; set; }

    [FromRoute]
    public string ProductName { get; set; } = string.Empty;

    [FromQuery]
    public int CurrentPage { get; set; } = 0;

    [FromQuery]
    public string? From { get; set; }

    [FromQuery]
    public string? To { get; set; }

    [FromQuery]
    public int? FiscalYear { get; set; } = 0;


    public PageResult<IEnumerable<ProductProfit>> Result { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        long userFiscalYear = FiscalYear > 0 ? (long)FiscalYear : long.Parse(User.FindFirstValue("Last_Fiscal_Year"));
        var pageInfoResult = await _mediator.Send(new GetAllProductProfitsCountQuery(userFiscalYear, ProductId, From, To));
        if (pageInfoResult.Succeeded)
        {
            Result.PageInfo = new(CurrentPage, 50, pageInfoResult.Result);

            var fetchAllProductProfit = await _mediator.Send(new GetAllProductProfitsQuery(userFiscalYear, ProductId, From, To, CurrentPage * 50));
            if (fetchAllProductProfit.Succeeded)
            {
                Result.Result = fetchAllProductProfit.Result;
                return Page();
            }
        }

        return Redirect("/");
    }
}
