using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Commands.Factor;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.Query.Factor;
using SaysanPwa.Domain.SeedWorker;
using System.Security.Claims;

namespace SaysanPwa.Api.Pages.SalesFactorMoadian
{
	public class IndexModel : PageModel
	{
		private readonly IMediator _mediator;

		public IndexModel(IMediator mediator)
		{
			_mediator = mediator;
			TaxSaleListsentDto = new();
		}

		[FromQuery]
		public int CurrentPage { get; set; } = 0;

		[FromQuery]
		public string? From { get; set; }

		[FromQuery]
		public string? To { get; set; }


		public PageResult<List<TaxSaleDto>?> PageResult { get; set; } = new();


		[BindProperty]
		[FromForm]
		public List<TaxSaleDto> TaxSaleListsentDto { get; set; }

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
			var totalItems = await _mediator.Send(new GetAllSalesFactorMoadianQuery(ID_tbl_SalMaly, Date1, Date2));
			TaxSaleListsentDto = await _mediator.Send(new GetAllSalesFactorMoadianSendListQuery(ID_tbl_SalMaly, Date1, Date2));

			PageResult.PageInfo = new(CurrentPage, 50, totalItems.Count());

			PageResult.Result = totalItems;
		}


		[BindProperty]
		[FromForm]
		public TaxSaleDto taxSaleListsentDto { get; set; } = new();
		public async Task<IActionResult> OnPostSendFactorAsync()
		{
			var subjectsorathesab = taxSaleListsentDto.Subject_Sorathesab;
			var typesorathesab = taxSaleListsentDto.Type_Sorathesab;
			var ID_FF = taxSaleListsentDto.ID_tbl_FF;
			int ID_tbl_SalMaly = Convert.ToInt32(User.FindFirstValue("Last_Fiscal_Year"));
			var result = await _mediator.Send(new GetCkeckSaleFactorMoadian(ID_FF, typesorathesab, subjectsorathesab, ID_tbl_SalMaly));


			//return Redirect("/SalesFactorMoadian/Index");
			//var result = await _mediator.Send(_mapper.Map<EditServiceSaleFactorCommand>(EditServiceSaleFactorDto));
			if (result!=null)
			{
				TempData["Status"] = result;
				return Redirect("/SalesFactorMoadian/Index");
			}
			else
			{
				TempData["Status"] ="????? ?? ?????? ??????? ?? ???? ???";
				return Redirect("/SalesFactorMoadian/Index");
			}
		}




		[BindProperty]
		[FromForm]
		public TaxSaleDto SaleListsentDto { get; set; } = new();
		public async Task<IActionResult> OnPostResultPostAsync()
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
			var totalItems = await _mediator.Send(new GetAllSalesFactorMoadianQuery(ID_tbl_SalMaly, Date1, Date2));
			TaxSaleListsentDto = await _mediator.Send(new GetAllSalesFactorMoadianSendListQuery(ID_tbl_SalMaly, Date1, Date2));

			PageResult.PageInfo = new(CurrentPage, 50, totalItems.Count());

			PageResult.Result = totalItems;
			return Redirect("/SalesFactorMoadian/Index");
			//var result = await _mediator.Send(_mapper.Map<EditServiceSaleFactorCommand>(EditServiceSaleFactorDto));
			//if (result.Succeeded)
			//{
			//	TempData["Status"] = "Update Successed";
			//	return Redirect("/ServiceSaleFactor/AddFactor");
			//}
			//else
			//{
			//	TempData["Status"] = result.ErrorMessages.First();
			//	return Redirect("/ServiceSaleFactor/AddFactor");
			//}
		}
	}
}
