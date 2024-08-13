using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Application.Query.Partnership;

namespace SaysanPwa.Api.Pages.Partnership;


[Authorize]
public class DetailModel : PageModel
{
    private readonly IMediator _mediator;

    public DetailModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public PartnerDetailDto Partner { get; set; } = null!;
    public async Task OnGet(int id)
    {
        Partner = await _mediator.Send(new GetPartnerDetailQuery(id));
    }
}
