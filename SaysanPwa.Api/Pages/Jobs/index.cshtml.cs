using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.Jobs;
using SaysanPwa.Application.Query.Jobs;

namespace SaysanPwa.Api.Pages.Jobs
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<GetAllJobsResponseDto>? Jobs { get; set; }
        public async Task OnGet()
        {
            Jobs = await _mediator.Send(new GetAllJobsQuery());
        }
    }
}
