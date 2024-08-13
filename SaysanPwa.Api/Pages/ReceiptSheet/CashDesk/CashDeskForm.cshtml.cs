using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.Query.ReceiptSheet;
using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SaysanPwa.Api.Pages.ReceiptSheet.CashDesk;

public class CashDeskFormModel : PageModel
{
    private readonly IMediator _mediator;

    public CashDeskFormModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public class CashDeskFormInputModel
    {
        [Required(ErrorMessage = "لطفا یک صندوق را انتخاب کنید.")]
        public int CashDeskId { get; set; }
        [Required(ErrorMessage = "لطفا میلغ را وارد کنید.")]
        public decimal Price { get; set; }
        public string? Description { get; set; } = "";
    }


    [BindProperty, FromForm]
    public CashDeskFormInputModel FormInput { get; set; }

    [BindNever]
    public IEnumerable<tbl_Sandog> CashDesks { get; set; } = Enumerable.Empty<tbl_Sandog>();

    [FromRoute]
    public string SessionId { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        if (SessionId == null || SessionId == string.Empty)
        {
            return Content("اطلاعات به درستی بارگذاری نشده است.");
        }
        var data = HttpContext.Session.GetString(SessionId);
        ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(data)!;
        if (dto.CashDesks.Any())
        {
            return Content("اپتدا دریافت صندوق قبلی را حذف نمایید.");
        }
        else
        {
            CashDesks = await _mediator.Send(new GetAllCashDesksQuery());
            return Page();
        }
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            var data = HttpContext.Session.GetString(SessionId);
            if (string.IsNullOrEmpty(data))
            {
                TempData["Error"] = "اعتبار ثبت شما تمام شده است. لطفا مراحل را از اول طی کنید.";
                return RedirectToPage("/ReceiptSheet/Index");
            }
            ReceiptSheetBaseDto dto = JsonSerializer.Deserialize<ReceiptSheetBaseDto>(data)!;
            dto.CashDesks.Add(new()
            {
                ItemId = Guid.NewGuid(),
                CashDeskId = FormInput.CashDeskId,
                Description = FormInput.Description,
                Price = FormInput.Price
            });
            HttpContext.Session.SetString(SessionId, JsonSerializer.Serialize(dto));
            return Content("OK");
        }
        return null;
    }
}
