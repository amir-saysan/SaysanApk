using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaysanPwa.Application.Commands.Partnership;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Application.Query.Partnership;

namespace SaysanPwa.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class PartnerController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PartnerController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("DeleteBranchById/{id:long}")]
    public async Task<IActionResult> DeleteBranchById([FromRoute] long id)
    {
        var canDeleteBranchResult = await _mediator.Send(new CanDeletePartnerBranchQuery((int)id, 0));
        if (canDeleteBranchResult.Succeeded)
        {
            var result = await _mediator.Send(new DeleteBranchByIdCommand(id));
            return Ok(result.Result);
        }
        else
        {
            return Ok(canDeleteBranchResult.Result);
        }
    }


    [HttpGet("get-all-partner-groups")]
    [Produces("application/json")]
    public async Task<IEnumerable<GetAllPartnerGroupsResponseDto>> GetAllPartnerGroups()
    {
        try
        {
            var result = await _mediator.Send(new GetAllPartnerGroupsQuery());
            return result;
        }
        catch (Exception ex) 
        {
            return Enumerable.Empty<GetAllPartnerGroupsResponseDto>();
        }
    }

    [HttpPost("add-new-partner-group/{title}")]
    [Produces("application/json")]
    public async Task<bool> AddNewPartnerGroup([FromRoute] string title)
    {
        if (!string.IsNullOrEmpty(title))
        {
            var result = await _mediator.Send(new AddNewPartnerGroupCommand(title));
            return result.Result;
        }
        return false;
    }

    //[HttpGet("getpartnersforfactor")]
    //[Produces("application/json")]
    //public async Task<IEnumerable<GetPartnersForFactorDto>> GetPartnersForFactor()
    //{
    //    return await _mediator.Send(new GetPartnersForFactorQuery());
    //}

    [HttpGet("getpartnersforfactor")]
    [Produces("application/json")]
    public async Task<IEnumerable<GetPartnersForFactorResponseDto>> GetPartnersForFactor()
    {
        var result = await _mediator.Send(new GetAllPartnersQuery(null, null));
        return _mapper.Map<IEnumerable<GetPartnersForFactorResponseDto>>(result);
    }

    [HttpGet("getPartnerBranchesForFactor")]
    [Produces("application/json")]
    public async Task<IEnumerable<GetPartnerBranchesForFactorResponseDto>> GetPartnerBranchesForFactor(long partnerId)
    {
        var result = await _mediator.Send(new GetPartnerBranchesForFactorQuery(partnerId));
        return result;
    }
} 