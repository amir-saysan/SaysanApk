using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Commands.VisitPathCommands;
using SaysanPwa.Application.DTOs.VisitPath;
using SaysanPwa.Application.Query.VisitPathQueries;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.VisitPath
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
            Paths = new List<GetPathsForUserResponseDto>();
            //EditPath = new();
        }

        public IEnumerable<GetPathsForUserResponseDto> Paths { get; set; }
        [BindProperty]
        public EditPathDto EditPath { get; set; }

        public async Task OnGet()
        {
            Paths = await _mediator.Send(new GetPathsForBazaryabQuery(long.Parse(User.FindFirstValue("ID_tbl_Bzy")?.ToString() ?? "0")));
        }

        public async Task<IActionResult> OnPost()
        {
            await _mediator.Send(new PathVisitedCommand(long.Parse(User.FindFirstValue("ID_tbl_Bzy")?.ToString() ?? "0"), EditPath.ID_tbl_TarafHesab, EditPath.ID_tbl_Partner_Branch, description:EditPath.Description_Visited!));
            return RedirectToPage("/visitpath/index");
        }
    }
}
