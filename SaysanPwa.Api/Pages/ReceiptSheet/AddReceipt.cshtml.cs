using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaysanPwa.Application.Commands.ReceiptSheet;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.Query.Factor;
using System.Security.Claims;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet;

public class AddReceiptModel : PageModel
{
    private readonly IMediator _mediator;

    public AddReceiptModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty, FromForm]
    public ReceiptSheetBaseDto ReceiptSheet { get; set; } = new();

    public IEnumerable<GetPartnersForFactorDto> Partners { get; set; } = Enumerable.Empty<GetPartnersForFactorDto>();

    public async Task<IActionResult> OnGetAsync()
    {
        Partners = await _mediator.Send(new GetPartnersForFactorQuery());
        string sessionKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
        HttpContext.Session.SetString(sessionKey, JsonSerializer.Serialize(ReceiptSheet));
        ReceiptSheet.SessionId = sessionKey;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(HttpContext.Session.GetString(ReceiptSheet.SessionId)!)!;
            dto.PartnerId = ReceiptSheet.PartnerId;
            dto.Date = ReceiptSheet.Date;
            dto.ReceiptionType = ReceiptSheet.ReceiptionType;
            dto.PartnerId = ReceiptSheet.PartnerId;
            dto.Description = ReceiptSheet.Description;
            dto.FiscalYear = User.FindFirstValue("Last_Fiscal_Year")!;
            dto.CurrentUser = int.Parse(User.FindFirstValue("ID_tbl_TarafHesab")!);

            if (dto.Checks.Any() || dto.CashDesks.Any() || dto.Remittances.Any() || dto.CardReader.Any())
            {

                var result = await _mediator.Send(new AddReceiptSheetCommand()
                {
                    ReceiptSheetBaseDto = dto
                });

                if (result.Result)
                {
                    TempData["CustomMessage"] = "ثبت با موفقیت انجام شد.";
                    HttpContext.Session.Remove(dto.SessionId);
                    return Redirect("/");
                }
                else
                {
                    foreach (string error in result.ErrorMessages)
                    {
                        ModelState.AddModelError("All", error);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("All", "هیچ دریافتی انجام نشده است.");
            }

        }
        Partners = await _mediator.Send(new GetPartnersForFactorQuery());
        return Page();
    }
}