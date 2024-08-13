using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.Check;

public class DeleteCheckModel : PageModel
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
            int index = dto.Checks.FindIndex(x => x.ItemId.Equals(itemId));
            if (index >= 0)
            {
                dto.Checks.RemoveAt(index);
                HttpContext.Session.SetString(SessionId, JsonSerializer.Serialize(dto));
                return Content("اطلاعات با موفقیت حذف شد.");
            }
        }
        return Content("اطلاعات به درستی وارد نشده است.");
    }
}
