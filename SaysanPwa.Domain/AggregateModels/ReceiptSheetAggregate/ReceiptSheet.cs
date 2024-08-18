using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate
{
	public class ReceiptSheet
	{
		public string ReceiptionType { get; set; } = string.Empty;
		public long PartnerId { get; set; }
		public string Date { get; set; } = string.Empty;
		public string? Description { get; set; } = "";
		public string SessionId { get; set; } = string.Empty;
		public string FiscalYear { get; set; } = string.Empty;
		public int CurrentUser { get; set; }
		public long ID_tbl_TarafHesab { get; set; }
		public string Name_TarafHesab { get; set; }
		public long ID_tbl_DA { get; set; } //آیدی دریافت 
		public string Dt_DA { get; set; } //تاریخ 
		public decimal J_DA { get; set; } //جمع دریافت 
		public decimal J_DA_Ch { get; set; } //جمع دریافت چک 
		public decimal J_DA_H { get; set; } //جمع دریافت حواله 
		public decimal J_DA_K { get; set; } //جمع دریافت کارتخوان 
		public decimal J_DA_Sayer { get; set; } //جمع دریافت سایر 
		public string Dc_DA { get; set; } //شرح 
		public string Typ_DA { get; set; } //شرح 
		public string Typee { get; set; }
		public string Number_Hesab { get; set; }
		public decimal J_DA_AZ { get; set; }
		public string Desc_S1 { get; set; }
		public decimal J_S1 { get; set; }


		//public string tbl_TarafHesab.Name_TarafHesab { get; set; } //طرفحساب دریافت 
		//public string tbl_Users.Username { get; set; } //نام کاربر ثبت کننده سیتم 

		//public List<CashDeskDto> CashDesks { get; set; } = new();
		//public List<CardReaderDto> CardReader { get; set; } = new();
		//public List<RemittanceDto> Remittances { get; set; } = new();
		//public List<CheckDto> Checks { get; set; } = new();
	}
}
