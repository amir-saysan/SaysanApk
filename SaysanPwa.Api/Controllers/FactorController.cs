using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.Query.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using System.Security.Claims;
using System.Text.Json;

namespace SaysanPwa.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class FactorController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public FactorController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("validateinputs")]
    [Consumes("application/json")]
    public async Task<FactorValidationResult> validateNewFactorInputs(FactorValidationRequest factorValidationRequest)
    {
        var query = _mapper.Map<FactorValidationQuery>(factorValidationRequest);
        query.IsServiceValidation = false;
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpPost("validateinputsService")]
    [Consumes("application/json")]
    public async Task<FactorValidationResult> validateNewFactorInputsService(FactorValidationRequest factorValidationRequest)
    {
        var query = _mapper.Map<FactorValidationQuery>(factorValidationRequest);
        query.IsServiceValidation = true;
        var result = await _mediator.Send(query);
        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(factorValidationRequest.Products));
        return result;
    }


    [HttpGet("GetCustomPricing/{productId:long}/{partnerId:long}")]
    [Produces("application/json")]
    public async Task<FactorPricing> GetCustomFactorPricing([FromRoute] long productId, [FromRoute] long partnerId)
    {
        int fiscalYear = int.Parse(User.FindFirstValue("Last_Fiscal_Year"));
        var query = new GetCustomPricingForProductQuery(TypeCallProcedure.Select_Price_IN_FF, partnerId, productId, fiscalYear);
        var result = await _mediator.Send(query);
        return result.Result;
    }


    [HttpGet("GetServiceCustomPricing/{productId:long}/{partnerId:long}")]
    [Produces("application/json")]
    public async Task<FactorPricing> GetServiceCustomFactorPricing([FromRoute] long productId, [FromRoute] long partnerId)
    {
        int fiscalYear = int.Parse(User.FindFirstValue("Last_Fiscal_Year"));
        var query = new GetCustomPricingForProductQuery(TypeCallProcedure.Select_Price_IN_FF1, partnerId, productId, fiscalYear);
        var result = await _mediator.Send(query);
        return result.Result;
    }


    [HttpGet("GetCustomPricing-CostOfProduct/{productId:long}/{partnerId:long}")]
    [Produces("application/json")]
    public async Task<FactorPricing> GetCustomFactorPricingCostOfProduct([FromRoute] long productId, [FromRoute] long partnerId)
    {
        int fiscalYear = int.Parse(User.FindFirstValue("Last_Fiscal_Year"));
        var query = new GetCustomPricingForProductQuery(TypeCallProcedure.Select_Last_Bah_Kala_to_next_year, partnerId, productId, fiscalYear);
        var result = await _mediator.Send(query);
        return result.Result;
    }
}