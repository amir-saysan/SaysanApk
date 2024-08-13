using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Commands.Factor;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.Query.Factor;
using System.Security.Claims;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.SalesFactor
{
    [Authorize]
    public class AddFactorModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AddFactorModel(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
            Products = new();
            Partners = new List<GetPartnersForFactorDto>();
        }
        public List<GetProductsForFactorDto>? Products { get; set; }
        public List<GetPartnersForFactorDto> Partners { get; set; }

        public async Task<IActionResult> OnGet([FromQuery]long partnerId)
        {
            if(partnerId == 0)
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
        public AddSaleFactorDto SaleFactor { get; set; } = new();

        public async Task<IActionResult> OnPost()
        {
            SaleFactor.ID_tbl_Users = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            SaleFactor.ID_tbl_Bzy = long.Parse(User.FindFirstValue("ID_tbl_Bzy"));
            SaleFactor.ID_tbl_SalMaly = Convert.ToInt32(User.FindFirstValue("Last_Fiscal_Year"));
            SaleFactor.FiscalYearBeginDate = User.FindFirstValue("FiscalYearBeginDate");
            SaleFactor.FiscalYearEndDate = User.FindFirstValue("FiscalYearEndDate");
            SaleFactor.FiscalYearTitle = User.FindFirstValue("FiscalYearTitle");


            Console.WriteLine(JsonSerializer.Serialize(SaleFactor));
            var result = await _mediator.Send(_mapper.Map<AddSaleFactorCommand>(SaleFactor));
            if (result.Succeeded)
            {
                TempData["Status"] = "ثبت فاکتور موفقیت آمیز بود.";
                return Redirect("/SalesFactor/AddFactor");
            }
            else
            {
                TempData["Status"] = result.ErrorMessages.First();
                return Redirect("/SalesFactor/AddFactor");
            }
        }
    }
}
