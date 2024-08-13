using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.Utilities.Validation;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.Remittances;

public class TableModel : PageModel
{
    [FromRoute]
    public string SessionId { get; set; } = string.Empty;


    [BindNever]
    public IEnumerable<RemittanceDto> Remittances { get; set; } = Enumerable.Empty<RemittanceDto>();


    public IActionResult OnGet()
    {
        if (!SessionId.IsNullOrEmpty() && HttpContext.Session.GetString(SessionId) != null)
        {
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(HttpContext.Session.GetString(SessionId)!)!;
            Remittances = dto.Remittances;
            return Page();
        }
        return Content("اطلاعاتبه درستی وارد نشده است.");
    }
}
