using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.DTOs.ReceiptSheet;

public class GetReceiptSheetDto
{
	public string ReceiptionType { get; set; } = string.Empty;
	public long PartnerId { get; set; }
	public string Date { get; set; } = string.Empty;
	public string? Description { get; set; } = "";
	public string SessionId { get; set; } = string.Empty;
	public string FiscalYear { get; set; } = string.Empty;
	public int CurrentUser { get; set; }
	public long ID_tbl_TarafHesab { get; set; }
	public int ID_tbl_Users { get; set; }
	public long ID_tbl_DA { get; set; }//آیدی دریافت 
	public string Dt_DA { get; set; } //تاریخ 
	public decimal J_DA { get; set; } //جمع دریافت 
	public decimal J_DA_Ch { get; set; } //جمع دریافت چک 
	public decimal J_DA_H { get; set; } //جمع دریافت حواله 
	public decimal J_DA_K { get; set; } //جمع دریافت کارتخوان 
	public decimal J_DA_Sayer { get; set; } //جمع دریافت سایر 
	public decimal J_DA_AZ { get; set; }
	public string Dc_DA { get; set; } //شرح 
	public string Name_TarafHesab { get; set; }
	public string Typ_DA { get; set; }
	public string Typee { get; set; }
	public string Number_Hesab { get; set; }

    public decimal J_S1 { get; set; }
	public string Desc_S1 { get; set; }

	public List<CashDeskDto> CashDesks { get; set; } = new();
	public List<CardReaderDto> CardReader { get; set; } = new();
	public List<RemittanceDto> Remittances { get; set; } = new();
	public List<CheckDto> Checks { get; set; } = new();
}