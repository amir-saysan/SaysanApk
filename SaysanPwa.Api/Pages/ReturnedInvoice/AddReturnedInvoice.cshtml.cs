using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Commands.Factor;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.Query.Factor;
using System.Security.Claims;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReturnedInvoice
{
	public class AddReturnedInvoiceModel : PageModel
	{

		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public AddReturnedInvoiceModel(IMediator mediator, IMapper mapper)
		{
			_mediator = mediator;
			_mapper = mapper;
			Products = new();
			Partners = new List<GetPartnersForFactorDto>();
		}
		public List<GetProductsForFactorDto>? Products { get; set; }
		public List<GetPartnersForFactorDto> Partners { get; set; }

		public async Task<IActionResult> OnGet([FromQuery] long partnerId)
		{
			if (partnerId == 0)
			{
				Partners = (await _mediator.Send(new GetPartnersForFactorQuery())).ToList();
			}
			else
			{
				var partners = await _mediator.Send(new GetPartnersForFactorQuery());
				var partner = partners.Where(u => u.ID_tbl_TarafHesab == partnerId).FirstOrDefault();
				if (partner is not null)
				{
					Partners.Add(partner);
				}
				else
				{
					return BadRequest();
				}
			}
			Products = (await _mediator.Send(new GetProductsForFactorQuery())).ToList();
			return Page();
		}


        [BindProperty]
        [FromForm]
        public AddReturnedInvoiceDto ReturnedInvoice { get; set; } = new();


        public async Task<IActionResult> OnPost()
        {
            ReturnedInvoice.ID_tbl_Users = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ReturnedInvoice.ID_tbl_Bzy = long.Parse(User.FindFirstValue("ID_tbl_Bzy"));
            ReturnedInvoice.ID_tbl_SalMaly = Convert.ToInt32(User.FindFirstValue("Last_Fiscal_Year"));
            ReturnedInvoice.FiscalYearBeginDate = User.FindFirstValue("FiscalYearBeginDate");
            ReturnedInvoice.FiscalYearEndDate = User.FindFirstValue("FiscalYearEndDate");
            ReturnedInvoice.FiscalYearTitle = User.FindFirstValue("FiscalYearTitle");


            Console.WriteLine(JsonSerializer.Serialize(ReturnedInvoice));
            var result = await _mediator.Send(_mapper.Map<AddReturnedInvoiceCommand>(ReturnedInvoice));
            if (result.Succeeded)
            {
                TempData["Status"] = "ثبت فاکتور برگشتی موفقیت آمیز بود.";
                return Redirect("/ReturnedInvoice/AddReturnedInvoice");
            }
            else
            {
                TempData["Status"] = result.ErrorMessages.First();
                //return Redirect("/ReturnedInvoice/AddFactor");
                return Redirect("/ReturnedInvoice/AddReturnedInvoice");
            }
        }
    }
}
