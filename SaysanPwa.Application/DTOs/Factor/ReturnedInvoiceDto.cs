using SaysanPwa.Application.DTOs.Factor;

namespace SaysanPwa.Application.DTOs.ReturnedInvoice;

public class ReturnedInvoiceDto
{
	public long ID_tbl_FBB { get; set; }
	public string Type_tbl_F { get; set; } = "tbl_FBB";
	public string Type_Bargasht_Forosh { get; set; } = "Bargasht_Forosh_Dakheli";
	public long N_FBB { get; set; }
	public long ID_tbl_TarafHesab { get; set; }
	public string Name_TarafHesab { get; set; }
	public long ID_tbl_Partner_Branch { get; set; }
	public decimal Tkh_N_FBB { get; set; }
	public decimal Tkh_K_FBB { get; set; } = 0;
	public decimal Tkh_A_FBB { get; set; }
	public decimal JM_FBB { get; set; }
	public decimal JAv_FBB { get; set; }
	public decimal J_Hz_FBB { get; set; }
	public decimal J_F_FBB { get; set; }
	public decimal BG_FBB { get; set; }
	public decimal J_Tedad_Asl_FBB { get; set; }
	public decimal J_Tedad_Fare_FBB { get; set; }
	public long ID_tbl_FF { get; set; }
	public long ID_tbl_Bzy { get; set; }
	public decimal Prsnt_Bzy { get; set; } = 0;
	public string Dc_FBB { get; set; } = "";
	public string Sh_Sofa { get; set; } = "";
	public string Sh_R_Anb { get; set; } = "";
	public string Sh_F_Taraf { get; set; } = "";
	public string Dt_FBB { get; set; }
	public string Dt_C_FBB { get; set; }
	public string Tm_C_FBB { get; set; }
	public bool Conred_FBB { get; set; } = false;
	public long ID_tbl_PA { get; set; } = 0;
	public long ID_tbl_G1 { get; set; } = 0;
	public long ID_tbl_S1 { get; set; }
	public string IS_PR { get; set; } = "چاپ نشده";
	public int ID_tbl_SalMaly { get; set; }
	public int ID_tbl_Users { get; set; }
	public string Dt_Up { get; set; } = "";
	public string Tm_Up { get; set; } = "";
	public int ID_tbl_Users_Up { get; set; } = 0;



	public string Code_TarafHesab { get; set; }//کد مشتری
	public string ChelPhone_TarafHesab { get; set; }//تلفن مشتری
	public string Name_Bzy { get; set; }//نام بازاریاب
	public string Username { get; set; }//نام کاربر ثبت کننده سیتم
	public string Name_Kala { get; set; }//نام کالا
	public string BarCode_Kala { get; set; }//بارکد کالا
	public long Code_Kala { get; set; }//کد کالا
	public string Shenase_Kala { get; set; }//شناسه مالیاتی کالا
	public string Vahede_Name { get; set; }//واحد کالا
	public string Name_Anbar { get; set; }//نام انبار
	public string Code_Anbar { get; set; }//کد انبار
	public decimal Tedad { get; set; }//تعداد
	public decimal Fi { get; set; }//فی
	public decimal Mablagh { get; set; }//مبلغ کل
	public string Name_Branch { get; set; }//نام شعبه


	// ------------- For set fiscal year information to sale factor.

	public string FiscalYearBeginDate { get; set; } = string.Empty;
	public string FiscalYearEndDate { get; set; } = string.Empty;
	public string FiscalYearTitle { get; set; } = string.Empty;


	public IEnumerable<AddFactorItemDto> Items { get; set; } = new List<AddFactorItemDto>();
}