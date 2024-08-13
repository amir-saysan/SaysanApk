using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.Query.Factor;
using SaysanPwa.Domain.SeedWorker;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.SalesFactor
{
    [Authorize]
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

        public PageResult<List<GetSaleFactorDto>?> PageResult { get; set; } = new();


        public async Task OnGet()
        {
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
            int userid = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int ID_tbl_SalMaly = Convert.ToInt32(User.FindFirstValue("Last_Fiscal_Year"));
            var totalItems = await _mediator.Send(new GetAllSaleFactorsCountQuery(ID_tbl_SalMaly, userid, Date1, Date2));
            PageResult.PageInfo = new(CurrentPage, 50, totalItems.Count());

            PageResult.Result = totalItems;
        }
    }
}
