using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.CashDesk;

public class TableModel : PageModel
{
    [FromRoute]
    public string SessionId { get; set; } = string.Empty;

    [BindNever]
    public List<CashDeskDto> CashDesks { get; set; } = new();

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(SessionId))
        {
            return Content("عدم دریافت صحیح اطلاعات");
        }
        var data = HttpContext.Session.GetString(SessionId);
        if (!string.IsNullOrEmpty(data))
        {
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(data);
            CashDesks = dto!.CashDesks;
            return Page();
        }
        return Content("عدم دریافت صحیح اطلاعات");
    }
}
