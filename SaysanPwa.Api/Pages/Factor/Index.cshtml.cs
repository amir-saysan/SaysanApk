using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Query.Products;
using SaysanPwa.Application.Query.Factor;
using System.Security.Claims;
using SaysanPwa.Application.DTOs.Product;
using SaysanPwa.Domain.SeedWorker;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc;

namespace SaysanPwa.Api.Pages.Factor
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public int CurrentPage { get; set; } = 0;

        [FromQuery]
        public string? From { get; set; }

        [FromQuery]
        public string? To { get; set; }


        public PageResult<List<FactorDto>?> PageResult { get; set; } = new();
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
            var Result = await _mediator.Send(new GetFactorQuery(ID_tbl_SalMaly, userid, Date1, Date2));
            PageResult.PageInfo = new(CurrentPage, 50, Convert.ToInt32(Result.Count()));
            PageResult.Result = Result;

        }
    }
}
