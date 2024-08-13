using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.PosMachine;

public class DeletePosModel : PageModel
{
    [FromRoute]
    public string ItemId { get; set; } = string.Empty;
    [FromRoute]
    public string SessionId { get; set; } = string.Empty;
    public IActionResult OnGet()
    {
        if (Guid.TryParse(ItemId, out Guid itemId))
        {
            if (string.IsNullOrEmpty(ItemId) || HttpContext.Session.GetString(SessionId) == null)
            {
                return Content("اطلاعات به درستی وارد نشده است.");
            }

            var data = HttpContext.Session.GetString(SessionId);
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(data)!;
            int index = dto.CardReader.FindIndex(x => x.ItemId.Equals(itemId));
            if (index >= 0)
            {
                dto.CardReader.RemoveAt(index);
                HttpContext.Session.SetString(SessionId, JsonSerializer.Serialize(dto));
                return Content("اطلاعات با موفقیت حذف شد.");
            }
        }
        return Content("اطلاعات به درستی وارد نشده است.");
    }
}
