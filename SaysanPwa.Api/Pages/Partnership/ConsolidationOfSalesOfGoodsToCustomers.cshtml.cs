using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Query.Partnership;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.Partnership;


[Authorize]
public class ConsolidationOfSalesOfGoodsToCustomersModel : PageModel
{
    private readonly IMediator _mediator;

    public ConsolidationOfSalesOfGoodsToCustomersModel(IMediator mediator) => _mediator = mediator;

    [FromRoute]
    public int PartnerId { get; set; }

    [FromRoute]
    public string Name { get; set; } = string.Empty;

    [FromQuery]
    public long FiscalYear { get; set; } = 0;

    [FromQuery]
    public long MarketerId { get; set; } = 0;

    [FromQuery]
    public long Offset { get; set; } = 0;

    [FromQuery]
    public string? FromDate { get; set; } = null!;

    [FromQuery]
    public string? ToDate { get; set; } = null!;


    public IEnumerable<PartnerConsolidationOfSalesOfGoodsToCustomers> Data { get; set; } = Enumerable.Empty<PartnerConsolidationOfSalesOfGoodsToCustomers>();

    public async Task<IActionResult> OnGetAsync()
    {
        long userFiscalYear = FiscalYear > 0 ? (long)FiscalYear : long.Parse(User.FindFirstValue("Last_Fiscal_Year"));
        var result = await _mediator.Send(
            new GetAllPartnerConsolidationOfSalesOfGoodsToCustomersQuery(PartnerId, userFiscalYear, MarketerId > 0 ? MarketerId : null, Offset, FromDate, ToDate));
        if (result.Succeeded)
        {
            Data = result.Result;
            return Page();
        }
        return Redirect("/");
    }
}
