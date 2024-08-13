using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReturnedInvoice;
using SaysanPwa.Application.Query.ReturnedInvoice;
using SaysanPwa.Domain.SeedWorker;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.ReturnedInvoice
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

        [FromQuery]
        public string? From { get; set; }

        [FromQuery]
        public string? To { get; set; }

        public PageResult<List<ReturnedInvoiceDto>?> PageResult { get; set; } = new();

        public async Task OnGet()
        {
            int userid = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int ID_tbl_SalMaly = Convert.ToInt32(User.FindFirstValue("Last_Fiscal_Year"));
            string? Date1;
            string? Date2;
            if (From == null && To == null)
            {
                Date1 = Application.Utilities.DateAndTime.DateTimeExtensions.ConvertMiladiToShamsi(DateTime.Now, "yyyy/MM/dd");
                Date2 = Application.Utilities.DateAndTime.DateTimeExtensions.ConvertMiladiToShamsi(DateTime.Now, "yyyy/MM/dd");
            }
            else
            {
                Date1 = From;
                Date2 = To;
            }
            var returnedInvoiceDtos = await _mediator.Send(new GetReturnedInvoiceQuery(ID_tbl_SalMaly, userid, Date1, Date2));
            PageResult.PageInfo = new(CurrentPage, 50, returnedInvoiceDtos.Count());
            PageResult.Result = returnedInvoiceDtos;
        }
    }
}
