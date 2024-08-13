using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.Query.ReceiptSheet;
using SaysanPwa.Application.Utilities.Validation;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.PosMachine;

public class AddPosReceiveModel : PageModel
{
    private readonly IMediator _mediator;

    public AddPosReceiveModel(IMediator mediator) => _mediator = mediator;
    
    [FromRoute]
    public string SessionId { get; set; } = string.Empty;

    [FromForm]
    public CardReaderDto CardReader { get; set; } = new();

    public IDictionary<int, string> BackAccounts { get; set; } = new Dictionary<int, string>();

    public async Task<IActionResult> OnGetAsync()
    {
        BackAccounts = (await _mediator.Send(new GetAllBankAccountsQuery())).ToDictionary(o => o.ID_tbl_Hesab, o => o.Number_Hesab);
        return Page();
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid && !SessionId.IsNullOrEmpty() && HttpContext.Session.GetString(SessionId) != null)
        {
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(HttpContext.Session.GetString(SessionId)!)!;
            CardReader.ItemId = Guid.NewGuid();
            dto.CardReader.Add(CardReader);
            HttpContext.Session.SetString(SessionId, JsonSerializer.Serialize(dto));
        }
        return null!;
    }
}
