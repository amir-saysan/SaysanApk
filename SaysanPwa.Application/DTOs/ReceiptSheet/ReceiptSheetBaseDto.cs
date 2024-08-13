﻿using System.ComponentModel.DataAnnotations;

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
	public long ID_tbl_TarafHesab { get; set; }
	public int ID_tbl_Users { get; set; }
	public long ID_tbl_DA { get; set; }//آیدی دریافت 
	//public string ID_tbl_DA { get; set; } //شماره دریافت 
	public string Dt_DA { get; set; } //تاریخ 
	public decimal J_DA { get; set; } //جمع دریافت 
	public decimal J_DA_Ch { get; set; } //جمع دریافت چک 
	public decimal J_DA_H { get; set; } //جمع دریافت حواله 
	public decimal J_DA_K { get; set; } //جمع دریافت کارتخوان 
	public decimal J_DA_Sayer { get; set; } //جمع دریافت سایر 
	public decimal J_DA_AZ { get; set; }
	public decimal Desc_S1 { get; set; }
	public string Dc_DA { get; set; } //شرح 
	//public string tbl_TarafHesab.Name_TarafHesab { get; set; } //طرفحساب دریافت 
	//public string tbl_Users.Username { get; set; } //نام کاربر ثبت کننده سیتم
	public string Name_TarafHesab { get; set; }
	public string Typ_DA { get; set; }
	public string Typee { get; set; }
	public string Number_Hesab { get; set; }


	public List<CashDeskDto> CashDesks { get; set; } = new();
	public List<CardReaderDto> CardReader { get; set; } = new();
	public List<RemittanceDto> Remittances { get; set; } = new();
	public List<CheckDto> Checks { get; set; } = new();
}