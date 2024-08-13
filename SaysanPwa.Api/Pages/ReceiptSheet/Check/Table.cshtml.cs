using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.Utilities.Validation;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.Check;

public class TableModel : PageModel
{

    [FromRoute]
    public string SessionId { get; set; } = string.Empty;

    public List<CheckDto> Checks { get; set; } = new();




    public IActionResult OnGet()
    {
        if (!SessionId.IsNullOrEmpty() && HttpContext.Session.GetString(SessionId) != null)
        {
            ReceiptSheetBaseDto baseDto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(HttpContext.Session.GetString(SessionId)!)!;
            if (baseDto != null)
            {
                Checks = baseDto.Checks;
                return Page();
            }
        }
        return Content("اطلاعات به درستی وارد نشده است.");
    }
}