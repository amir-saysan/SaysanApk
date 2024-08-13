using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Query.Partnership;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.Partnership;

[Authorize]
public class PartnerReturnedChecksModel : PageModel
{
    private readonly IMediator _mediator;

    public PartnerReturnedChecksModel(IMediator mediator) => _mediator = mediator;


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


    public PageResult<IEnumerable<Check>> Result { get; set; } = new();


    public async Task<IActionResult> OnGetAsync()
    {
        long userFiscalYear = FiscalYear > 0 ? (long)FiscalYear : long.Parse(User.FindFirstValue("Last_Fiscal_Year"));

        var pageCountRequest = await _mediator.Send(new GetAllChecksCountByPartnerIdQuery(rdoEnglish_V_Ch.rdoBargashti, userFiscalYear, PartnerId, From, To));

        if (pageCountRequest.Succeeded)
        {
            Result.PageInfo = new(CurrentPage, 50, pageCountRequest.Result);
            var result = await _mediator.Send(new GetAllChecksQuery(rdoEnglish_V_Ch.rdoBargashti, userFiscalYear, PartnerId, From, To, CurrentPage * 50));
            if (result.Succeeded)
            {
                Result.Result = result.Result;
                return Page();
            }
        }
        return RedirectToPage("/Partnership/Index");
    }
}
