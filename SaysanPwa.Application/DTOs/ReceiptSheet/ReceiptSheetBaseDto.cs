using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.DTOs.ReceiptSheet;

public class ReceiptSheetBaseDto
{
	[Required(ErrorMessage = "لطفا یک نوع دریافت را انتخاب نمائید.")]
	public string ReceiptionType { get; set; } = string.Empty;

	[Required(ErrorMessage = "لطفا یک طرف حساب را انتخاب نمائید.")]
	//public string? PartnerName { get; set; } = string.Empty;
	public long PartnerId { get; set; }

	[Required(ErrorMessage = "لطفا تاریخ دریافت انتخاب نمائید.")]
	public string Date { get; set; } = string.Empty;

	public string? Description { get; set; } = "";


	public string SessionId { get; set; } = string.Empty;

	public string FiscalYear { get; set; } = string.Empty;

	public int CurrentUser { get; set; }

	public List<CashDeskDto> CashDesks { get; set; } = new();
	public List<CardReaderDto> CardReader { get; set; } = new();
	public List<RemittanceDto> Remittances { get; set; } = new();
	public List<CheckDto> Checks { get; set; } = new();
}