using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.Query.ReceiptSheet;
using SaysanPwa.Application.Utilities.Validation;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.Remittances;

public class AddRemittanceModel : PageModel
{
    private readonly IMediator _mediator;

    public AddRemittanceModel(IMediator mediator) => _mediator = mediator;

    [FromRoute]
    public string SessionId { get; set; } = string.Empty;

    [BindProperty, FromForm]
    public RemittanceDto RemmitanceInput{ get; set; }


    [BindNever]
    public IDictionary<int, string> BackAccount { get; set; } = new Dictionary<int, string>();


    public async Task<IActionResult> OnGetAsync()
    {
        if (!SessionId.IsNullOrEmpty() || HttpContext.Session.GetString(SessionId) != null)
        {
            BackAccount = (await _mediator.Send(new GetAllBankAccountsQuery())).ToDictionary(x => x.ID_tbl_Hesab, x => x.Number_Hesab);
            return Page();
        }
        return Content("اطلاعات به درستی وارد نشده است.");
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid && !SessionId.IsNullOrEmpty() && HttpContext.Session.GetString(SessionId) != null)
        {
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(HttpContext.Session.GetString(SessionId)!)!;
            RemmitanceInput.ItemId = Guid.NewGuid();
            dto.Remittances.Add(RemmitanceInput);
            HttpContext.Session.SetString(SessionId, JsonSerializer.Serialize(dto));
        }
        return null!;
    }
}
