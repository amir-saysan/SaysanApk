using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaysanPwa.Application.Query.Location;
using SaysanPwa.Application.Services;
using static SaysanPwa.Application.Services.LocationService;

namespace SaysanPwa.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LocationController> _logger;

    public LocationController(ILogger<LocationController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("GetRegions")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Region>))]
    public async Task<IEnumerable<Region>> GetRegions()
    {
        return await LocationService.GetRegions();
    }

    [HttpGet("GetCities")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<City>))]
    public async Task<IEnumerable<Domain.AggregateModels.LocationAggregate.City>> GetCities(int regionId)
    {
        return await _mediator.Send(new GetCitiesQuery(regionId));
    }


    [HttpGet("get-city-name-by-id/{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(CityNameDto))]
    public async Task<CityNameDto> GetCityNameById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetCityNameByCityIdQuery(id));
        if (result.Succeeded)
        {
            return new(result.Result);
        }
        return new("-");
    }


    [HttpGet("get-region-name-by-id/{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(RegionNameDto))]
    public async Task<RegionNameDto> GetRegionNameById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetRegionNameByRegionIdQuery(id));
        if (result.Succeeded)
        {
            return new(result.Result);
        }
        return new("-");
    }

    public class CityNameDto
    {
        public string Name { get; set; } = string.Empty;

        public CityNameDto(string name) => Name = name;
    }

    public class RegionNameDto
    {
        public string Name { get; set; } = string.Empty;

        public RegionNameDto(string name) => Name = name;
    }
}
