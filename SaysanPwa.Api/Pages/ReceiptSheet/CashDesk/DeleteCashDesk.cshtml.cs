using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.CashDesk;

public class DeleteCashDeskModel : PageModel
{
    [FromRoute(Name = "ItemId")]
    public string ItemId { get; set; }

    [FromRoute(Name = "SessionId")]
    public string SessionId { get; set; } = string.Empty;


    public IActionResult OnGet()
    {
        if (Guid.TryParse(ItemId, out Guid itemId))
        {
            if (string.IsNullOrEmpty(SessionId) && HttpContext.Session.GetString(SessionId) == null)
            {
                return Content("اطلاعات وارد شده نادرست میباشد.");
            }
            string data = HttpContext.Session.GetString(SessionId)!;
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(data)!;


            int index = dto.CashDesks.FindIndex(x => x.ItemId.Equals(itemId));
            if (index >= 0)
            {
                dto.CashDesks.RemoveAt(index);
                HttpContext.Session.SetString(SessionId, JsonSerializer.Serialize(dto));
                return Content("صندوق با موفقیت حذف شد.");
            }
        }
        return Content("اطلاعات وارد شده نادرست است.");
    }
}