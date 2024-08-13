using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Api.Logging;
using SaysanPwa.Application.Commands.Partnership;
using SaysanPwa.Application.DTOs.Jobs;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Application.Query.Jobs;
using SaysanPwa.Application.Query.Location;
using SaysanPwa.Application.Query.Partnership;
using SaysanPwa.Domain.AggregateModels.LocationAggregate;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.Partnership;

[AutoValidateAntiforgeryToken]
[Authorize]
public class CreateModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly ILoggerService _logger;

    public CreateModel(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
        Regions = new List<Region>();
        Cities = new List<City>();
        CreatePartnerModel = new();
        PartnerGroups = new List<GetAllPartnerGroupsResponseDto>();
        Jobs = new List<GetAllJobsResponseDto>();
        Branches = new List<AddBranchDto>();
        Regions = _mediator.Send(new GetRegionsQuery()).Result;
        PartnerGroups = _mediator.Send(new GetAllPartnerGroupsQuery()).Result;
        Jobs = _mediator.Send(new GetAllJobsQuery()).Result;
    }

    public IEnumerable<GetAllPartnerGroupsResponseDto> PartnerGroups { get; set; }
    public IEnumerable<GetAllJobsResponseDto> Jobs { get; set; }
    public IEnumerable<Region> Regions { get; set; }
    public IEnumerable<City> Cities { get; set; }
    public Guid BranchSessionId { get; set; }

    public async Task<IActionResult> OnGetGetCities(int regionId)
    { 
        var cities = await _mediator.Send(new GetCitiesQuery(regionId));
        return new JsonResult(cities);
    }

    [BindProperty]
    [FromForm]
    public CreatePartnerCommand CreatePartnerModel { get; set; } = null!;
    [BindProperty]
    [FromForm]
    public List<AddBranchDto> Branches { get; set; }


    public void OnGet()
    {
        this.BranchSessionId = Guid.NewGuid();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
        if (ModelState.IsValid)
        {
            CreatePartnerModel.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User id is not valid"));
            var result = await _mediator.Send(CreatePartnerModel);
            if (result.Succeeded)
            {
                _logger.LogInfo("new partner created successfully!");
                return RedirectToPage("/Partnership/Index");
            }
        }
        return Page();
    }
}
