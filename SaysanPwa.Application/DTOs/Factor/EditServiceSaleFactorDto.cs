using SaysanPwa.Application.DTOs.Partner;
using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.DTOs.Factor;

public record EditServiceSaleFactorDto
{

	public long ID_tbl_FF_KHed { get; set; }
	public decimal Tkh_N_FF_KHed { get; set; }
	public decimal Tkh_A_FF_KHed { get; set; }
	public decimal JM_FF_KHed { get; set; }
	public decimal JAv_FF_KHed { get; set; }
	public decimal J_Hz_FF_KHed { get; set; }
	public decimal J_FF_KHed { get; set; }
	public string Dc_FF_KHed { get; set; }
	public long ID_tbl_DA { get; set; }
	public int ID_tbl_Users { get; set; }
	public long ID_tbl_TarafHesab { get; set; }//آیدی مشتری
	public string Name_TarafHesab { get; set; }//نام مشتری
	public string Code_TarafHesab { get; set; }//کد مشتری
	public string Name_Branch { get; set; }//نام شعبه
	public long ID_tbl_TarafHesab_Moj_Rdf { get; set; }//آیدی مجری ردیف
	public string Name_TarafHesab_Moj_Rdf { get; set; }//نام مجری
	public string Code_TarafHesab_Moj_Rdf { get; set; }//کد مجری
	public decimal Pors_Mbg_Rdf { get; set; }//مبلغ پورسانت مجری 
	public long ID_tbl_Khedmat { get; set; }
	public long IS_A_M_Khedmat { get; set; }
	public string ID_tblVahede_Kala { get; set; }//آیدی واحد فروش خدمت
	public string Name_Khedmat { get; set; }//نام خدمت
	public long Cde_Khedmat { get; set; }//کد خدمت
	public long id_tbl_Anbar { get; set; }//آیدی انبار 
	public decimal Tedad { get; set; }//تعداد
	public decimal Pors_Dr_Rdf { get; set; }//درصد پورسانت
	public decimal Fi_Bed_Haz { get; set; }//فی بدون هزینه
	public decimal Fi_Ba_Takh { get; set; }// فی با تخفیف
	public decimal Fi_Ba_Haz { get; set; }// فی با هزینه و تخفیف
	public decimal M_T_Radf_K { get; set; }// مبلغ تخفیف ردیف
	public decimal D_T_Radf_K { get; set; }//درصد تخفیف ردیف
	public decimal MA_Radf_K { get; set; }//درصد مالیات ردیف خدمت
	public decimal Mbg_MA_Radf_K { get; set; }//مبلغ مالیات ردیف خدمت
	public decimal AV_Radf_K { get; set; }//درصد عوارض ردیف خدمت
	public decimal Mbg_AV_Radf_K { get; set; }//مبلغ عوارض ردیف خدمت
	public int V_Asl { get; set; }//تعداد با واحد اصلی
	public int V_Fare { get; set; }//تعداد با واحد فرعی
	public decimal M_KHLS { get; set; }//مبلغ ناخالض
	public decimal ID_tbl_F_Aglm_Khed { get; set; }//آیدی اقلام فروش فاکتور خدمت 

	public IEnumerable<AddSaleServiceFactorItemDto> Items { get; set; } = new List<AddSaleServiceFactorItemDto>();






	public string Type_tbl_F { get; set; } = "tbl_FF_KHed";
	public long N_FF_KHed { get; set; }
	public long ID_tbl_TarafHesab_Moj { get; set; } = 0;
	public decimal Pors_Mbg { get; set; } = 0;
	public bool rdo1 { get; set; } = false;
	public bool rdo2 { get; set; } = true;
	public decimal BG_FF_KHed { get; set; }
	public long J_Tedad_Asli_FF_KHed { get; set; }
	public long J_Tedad_Farei_FF_KHed { get; set; } = 0;
	public string Dt_FF_KHed { get; set; }
	public string Dt_C_FF_KHed { get; set; }
	public string Tm_C_FF_KHed { get; set; }
	public bool Conred_FF_KHed { get; set; } = false;
	public long ID_tbl_S1 { get; set; }
	public string IS_PR { get; set; } = "چاپ نشده";
	public long ID_tbl_Bzy { get; set; }
	public decimal Prsnt_Bzy { get; set; } = 0;
	public decimal Pdsh_Bzy { get; set; } = 0;
	public int ID_tbl_SalMaly { get; set; }
	public string Dt_Up { get; set; } = "";
	public string Tm_Up { get; set; } = "";
	public int ID_tbl_Users_Up { get; set; } = 0;

	// ------------- For init fiscal year information to sale factor.

	public string FiscalYearBeginDate { get; set; } = string.Empty;
	public string FiscalYearEndDate { get; set; } = string.Empty;
	public string FiscalYearTitle { get; set; } = string.Empty;
}
