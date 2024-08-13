using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Query.Partnership;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.Partnership;

[Authorize]
public class PartnerFactorsModel : PageModel
{
    private readonly IMediator _mediator;

    public PartnerFactorsModel(IMediator mediator)
    {
        _mediator = mediator;
    }


    [FromRoute]
    public long PartnerId { get; set; }

    [FromRoute]
    public string Name { get; set; } = string.Empty;

    [FromQuery]
    public int CurrentPage { get; set; } = 0;

    [FromQuery]
    public string? From { get; set; }

    [FromQuery]
    public string? To { get; set; }

    [FromQuery]
    public int? FiscalYear { get; set; } = 0;


    public PageResult<IEnumerable<SellReport>> Result { get; set; } = new();


    public async Task<IActionResult> OnGetAsync()
    {
        long userFiscalYear = FiscalYear > 0 ? (long)FiscalYear : long.Parse(User.FindFirstValue("Last_Fiscal_Year"));

        long marketerId = long.Parse(User.FindFirstValue("ID_tbl_Bzy"));
        

        var pageCountRequest = await _mediator.Send(new GetAllSellReportsByPartnerIdCountQuery(PartnerId, userFiscalYear, marketerId, From, To));
        if (pageCountRequest.Succeeded)
        {
            Result.PageInfo = new(CurrentPage, 50, pageCountRequest.Result);
            var result = await _mediator.Send(new GetAllSellReportsQuery(PartnerId, userFiscalYear, marketerId, CurrentPage * 50 , From, To));
            if (result.Succeeded)
            {
                Result.Result = result.Result;
                return Page();
            }
        }
        return RedirectToPage("/Partnership/Index");
    }
}
