using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.Utilities.Validation;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.PosMachine;

public class TableModel : PageModel
{

    [FromRoute]
    public string SessionId { get; set; } = string.Empty;

    [BindNever]
    public IEnumerable<CardReaderDto> CardReaders { get; set; } = Enumerable.Empty<CardReaderDto>();

    public IActionResult OnGet()
    {
        if (!SessionId.IsNullOrEmpty() && HttpContext.Session.GetString(SessionId) != null)
        {
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(HttpContext.Session.GetString(SessionId)!)!;
            CardReaders = dto.CardReader;
            return Page();
        }
        return Content("اطلاعات به درستی وارد نشده است.");
    }
}