using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaysanPwa.Api.Logging;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.DTOs.Product;
using SaysanPwa.Application.Query.Factor;
using SaysanPwa.Application.Query.Products;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;

namespace SaysanPwa.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductController : Controller
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly ILoggerService _logger;
    private readonly IMapper _mapper;

    public ProductController(IMediator mediator, IConfiguration configuration, ILoggerService logger, IMapper mapper)
    {
        _mediator = mediator;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionHandling.ApplicationException))]
    [Produces("application/json")]
    public async Task<List<ProductDto>> GetProducts()
    {
        return await _mediator.Send(new GetProductsQuery());
    }

    [HttpGet("getProductsForFactorSearch")]
    [Produces("application/json")]
    public async Task<IEnumerable<ProductsForSearchDto>> getProductsForFactorSearch()
    {
        var result = await _mediator.Send(new GetProductsQuery());
        return _mapper.Map<IEnumerable<ProductsForSearchDto>>(result);
    }

    [HttpGet("getServicesForFactorSearch")]
    [Produces("application/json")]
    public async Task<IEnumerable<ServicesForSearchDto>> getServicesForFactorSearch()
    {
        var result = await _mediator.Send(new GetAllServicesQuery());
        return _mapper.Map<IEnumerable<ServicesForSearchDto>>(result);
    }

    [HttpGet("productHasDiscount")]
    [Produces("application/json")]
    public async Task<HasDiscountViewModel> ProductHasDiscount(long productId)
    {
        return await _mediator.Send(new ProductHasDiscountQuery(productId, DiscountType.tbl_Kala));
    }

    [HttpGet("serviceHasDiscount")]
    [Produces("application/json")]
    public async Task<HasDiscountViewModel> ServiceHasDiscount(long productId)
    {
        return await _mediator.Send(new ProductHasDiscountQuery(productId, DiscountType.tbl_Khedmat));
    }
}