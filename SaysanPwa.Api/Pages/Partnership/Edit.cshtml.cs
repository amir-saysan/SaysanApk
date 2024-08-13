using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Commands.Partnership;
using SaysanPwa.Application.DTOs.Jobs;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Application.Query.Jobs;
using SaysanPwa.Application.Query.Location;
using SaysanPwa.Application.Query.Partnership;
using SaysanPwa.Domain.AggregateModels.LocationAggregate;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.Partnership;

[Authorize]
public class EditModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public EditModel(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        PartnerEditModel = new();
        _mapper = mapper;
        Regions = _mediator.Send(new GetRegionsQuery()).Result;
        Cities = new List<City>();
        PartnerGroups = _mediator.Send(new GetAllPartnerGroupsQuery()).Result;
        Jobs = _mediator.Send(new GetAllJobsQuery()).Result;
        //Branches = new();
    }

    //
    // 
    public IEnumerable<GetAllPartnerGroupsResponseDto> PartnerGroups { get; set; }
    public IEnumerable<GetAllJobsResponseDto> Jobs { get; set; }
    public IEnumerable<Region> Regions { get; set; }
    public IEnumerable<City> Cities { get; set; }

    //
    //

    [BindProperty]
    [FromForm]
    public EditPartnerRequestDto PartnerEditModel { get; set; }
    [BindProperty]
    [FromForm]
    public List<EditBranchDto> Branches { get; set; }

    public long LastBranchId { get; set; }
    //
    //

    public async Task<IActionResult> OnGet(long id)
    {
        if (id == 0)
            return NotFound();
        var partnerDetail = await _mediator.Send(new GetPartnerDetailQuery(id));
        PartnerEditModel = _mapper.Map<EditPartnerRequestDto>(partnerDetail);
        PartnerGroups = _mediator.Send(new GetAllPartnerGroupsQuery()).Result;
        Jobs = _mediator.Send(new GetAllJobsQuery()).Result;
        Regions = await _mediator.Send(new GetRegionsQuery());
        if (PartnerEditModel.ID_tbl_Ostan_Asli != null)
        {
            Cities = await _mediator.Send(new GetCitiesQuery((int)PartnerEditModel.ID_tbl_Ostan_Asli));
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
        ModelState.Remove("ID_tbl_Partner_Branch");
        if (ModelState.IsValid)
        {
            long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            EditPartnerCommand command = _mapper.Map<EditPartnerCommand>(PartnerEditModel);
            command.UserId = userId;

            var result = await _mediator.Send(command);
            return RedirectToPage("/Partnership/Index");
        }
        else
        {
            return Page();
        }
    }


}
