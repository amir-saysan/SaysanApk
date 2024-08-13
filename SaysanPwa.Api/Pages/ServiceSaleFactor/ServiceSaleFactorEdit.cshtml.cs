using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Commands.Factor;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.Query.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using System.Security.Claims;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ServiceSaleFactor
{
    public class ServiceSaleFactorEditModel : PageModel
    {

		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public ServiceSaleFactorEditModel(IMediator mediator, IMapper mapper)
		{
			_mediator = mediator;
			_mapper = mapper;
			Services = new();
			Partners = new GetPartnersForFactorDto();
			editServiceSaleFactorDto = new();
		}
		public List<Services>? Services { get; set; }
		public GetPartnersForFactorDto Partners { get; set; }



		public IEnumerable<PartnerInServiceFactor> PartnerInServiceFactors { get; set; }
		[BindProperty]
		[FromForm]
		public List<EditServiceSaleFactorDto> editServiceSaleFactorDto { get; set; }


		

		public async Task<IActionResult> OnGet(long id)
		{
			GetPartnersForFactorDto GetPartnersForFactorDto = new GetPartnersForFactorDto();
			int IDSalMaly = Convert.ToInt32(User.FindFirstValue("Last_Fiscal_Year"));
			if (id == 0)
				return NotFound();
			var partnerDetail = await _mediator.Send(new GetServiceSaleFactorDetailQuery(id, IDSalMaly));
			editServiceSaleFactorDto = _mapper.Map<List<EditServiceSaleFactorDto>>(partnerDetail);
			Partners.ID_tbl_TarafHesab = partnerDetail.FirstOrDefault().ID_tbl_TarafHesab;
			Partners.Name_TarafHesab = partnerDetail.FirstOrDefault().Name_TarafHesab;
			Partners.Code_TarafHesab = partnerDetail.FirstOrDefault().Code_TarafHesab;
			Partners.Name_Branch = partnerDetail.FirstOrDefault().Name_Branch;
			Services = (await _mediator.Send(new GetAllServicesQuery())).ToList();
			PartnerInServiceFactors = await _mediator.Send(new GetPartnersInServiceFactorQuery());

			return Page();


		}


		[BindProperty]
		[FromForm]
		public EditServiceSaleFactorDto EditServiceSaleFactorDto { get; set; } = new();

		public async Task<IActionResult> OnPost()
		{
			EditServiceSaleFactorDto.ID_tbl_Users = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			EditServiceSaleFactorDto.ID_tbl_SalMaly = Convert.ToInt32(User.FindFirstValue("Last_Fiscal_Year"));
			EditServiceSaleFactorDto.FiscalYearBeginDate = User.FindFirstValue("FiscalYearBeginDate");
			EditServiceSaleFactorDto.FiscalYearEndDate = User.FindFirstValue("FiscalYearEndDate");
			EditServiceSaleFactorDto.FiscalYearTitle = User.FindFirstValue("FiscalYearTitle");


			Console.WriteLine(JsonSerializer.Serialize(EditServiceSaleFactorDto));
			var result = await _mediator.Send(_mapper.Map<EditServiceSaleFactorCommand>(EditServiceSaleFactorDto));
			if (result.Succeeded)
			{
				TempData["Status"] = "?????? ???? ?? ?????? ????? ??.";
				return Redirect("/ServiceSaleFactor/AddFactor");
			}
			else
			{
				TempData["Status"] = result.ErrorMessages.First();
				return Redirect("/ServiceSaleFactor/AddFactor");
			}
		}
	}
}
