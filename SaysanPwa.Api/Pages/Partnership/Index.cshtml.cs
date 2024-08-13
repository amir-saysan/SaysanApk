using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Query.Partnership;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Api.Pages.Partnership;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }


    [FromQuery]
    public string? Query { get; set; }

    [FromQuery]
    public int CurrentPage { get; set; } = 0;

    public List<PartnerDetailViewModel> Partners { get; set; }


    public PageResult<IEnumerable<PartnerDetailViewModel>> Result { get; set; } = new();



    public async Task<IActionResult> OnGetAsync()
    {
        int fetchPartnerCount = await _mediator.Send(new GetAllPartnerCountQuery(Query!));
        Result.PageInfo = new(CurrentPage, 50, fetchPartnerCount);

        Result.Result = await _mediator.Send(new GetAllPartnersQuery(Query!, CurrentPage * 50));
        if (!string.IsNullOrEmpty(Query))
        {
            this.Partners = await _mediator.Send(new GetAllPartnersQuery(Query!, CurrentPage * 50)); ;
        }
        return Page();
    }
}
