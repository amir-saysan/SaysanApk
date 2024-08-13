using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.Commands.ReceiptSheet;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.Check;

public class AddCheckModel : PageModel
{
    [BindProperty, FromForm]
    public CheckDto CheckDto { get; set; } = new();

    [FromRoute]
    public string SessionId { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(SessionId))
        {
            return Content("اطلاعات به درستی وارد نشده است.");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionId)))
            {
                ReceiptSheetBaseDto baseDto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(HttpContext.Session.GetString(SessionId)!)!;
                if (baseDto != null)
                {
                    CheckDto.ItemId = Guid.NewGuid();
                    baseDto.Checks.Add(CheckDto);
                    HttpContext.Session.SetString(SessionId, JsonSerializer.Serialize(baseDto));
                }
            }
        }
        return null!;
    }
}