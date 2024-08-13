using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReturnedInvoice;
using SaysanPwa.Application.Query.Factor;
using SaysanPwa.Application.Query.ReturnedInvoice;
using SaysanPwa.Domain.AggregateModels.FiscalYear;
using SaysanPwa.Domain.SeedWorker;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.ReturnedInvoice
{
    public class PrintModel : PageModel
    {
        private readonly IMediator _mediator;

        public PrintModel(IMediator mediator)
        {
            _mediator = mediator;
        }


        [FromQuery]
        public int? FiscalYear { get; set; } = 0;
        public PageResult<List<ReturnedInvoiceDto>?> PageResult { get; set; } = new();

        public async Task OnGet(int FactorId)
        {
            long userFiscalYear = FiscalYear > 0 ? (long)FiscalYear : long.Parse(User.FindFirstValue("Last_Fiscal_Year"));
            var Result = await _mediator.Send(new GetReturnedFactorPrintQuery(FactorId, userFiscalYear, null, null, null, null, null));
            PageResult.PageInfo = new(0, 50, Result.Count());

            PageResult.Result = Result;


        }
    }
}
