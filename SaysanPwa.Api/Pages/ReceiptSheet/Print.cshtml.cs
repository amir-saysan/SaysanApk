using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.Query.ReceiptSheet;
using SaysanPwa.Domain.SeedWorker;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.ReceiptSheet
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
        public PageResult<List<ReceiptSheetBaseDto>?> PageResult { get; set; } = new();

        public async Task OnGet(int Id)
        {
            long userFiscalYear = FiscalYear > 0 ? (long)FiscalYear : long.Parse(User.FindFirstValue("Last_Fiscal_Year"));
            var Result = await _mediator.Send(new GetReceiptSheetPrintQuery(Id, userFiscalYear, null, null, null, null, null));
            PageResult.PageInfo = new(0, 50, Result.Count());

            PageResult.Result = Result;


        }
    }
}
