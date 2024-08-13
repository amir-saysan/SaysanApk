using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.Product;
using SaysanPwa.Application.Query.Products;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Api.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FromQuery]
        public int CurrentPage { get; set; } = 0;

        public PageResult<List<ProductDto>?> PageResult { get; set; } = new();

        public async Task OnGet()
        {
            int totalItems = await _mediator.Send(new GetAllProductsCountQuery());
            PageResult.PageInfo = new(CurrentPage, 50, totalItems);

            PageResult.Result = await _mediator.Send(new GetProductsQuery(CurrentPage * 50));
        }
    }
}
