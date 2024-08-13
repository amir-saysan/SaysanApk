namespace SaysanPwa.Application.DTOs.Factor;

public record AddServiceSaleFactorDto
{
    public string Type_tbl_F { get; set; } = "tbl_FF_KHed";
    public long N_FF_KHed { get; set; }
    public long ID_tbl_TarafHesab_Moj { get; set; } = 0;
    public decimal Pors_Mbg { get; set; } = 0;
    public bool rdo1 { get; set; } = false;
    public bool rdo2 { get; set; } = true;
    public decimal Tkh_N_FF_KHed { get; set; }
    public decimal Tkh_K_FF_KHed { get; set; } = 0;
    public decimal Tkh_A_FF_KHed { get; set; }
    public decimal JM_FF_KHed { get; set; }
    public decimal JAv_FF_KHed { get; set; }
    public decimal J_Hz_FF_KHed { get; set; } = 0;
    public decimal J_FF_KHed { get; set; }
    public decimal BG_FF_KHed { get; set; }
    public long J_Tedad_Asli_FF_KHed { get; set; }
    public long J_Tedad_Farei_FF_KHed { get; set; } = 0;
    public string Dc_FF_KHed { get; set; }
    public string Dt_FF_KHed { get; set; }
    public string Dt_C_FF_KHed { get; set; }
    public string Tm_C_FF_KHed { get; set; }
    public bool Conred_FF_KHed { get; set; } = false;
    public long ID_tbl_DA { get; set; } = 0;
    public long ID_tbl_S1 { get; set; }
    public string IS_PR { get; set; } = "چاپ نشده";
    public long ID_tbl_Bzy { get; set; }
    public decimal Prsnt_Bzy { get; set; } = 0;
    public decimal Pdsh_Bzy { get; set; } = 0;
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }
    public string Dt_Up { get; set; } = "";
    public string Tm_Up { get; set; } = "";
    public int ID_tbl_Users_Up { get; set; } = 0;

    // ------------- For init fiscal year information to sale factor.

    public string FiscalYearBeginDate { get; set; } = string.Empty;
    public string FiscalYearEndDate { get; set; } = string.Empty;
    public string FiscalYearTitle { get; set; } = string.Empty;

    public long ID_tbl_TarafHesab { get; set; }

    public IEnumerable<AddSaleServiceFactorItemDto> Items { get; set; } = new List<AddSaleServiceFactorItemDto>();
}


public record AddSaleServiceFactorItemDto
{
    public long ID_tbl_Khedmat { get; set; }
    public int ID_tblVahede_Kala { get; set; }
    public decimal Tedad { get; set; }
    public decimal Fi_Bed_Haz { get; set; }
    public decimal Fi_Ba_Takh { get; set; }
    public decimal Fi_Ba_Haz { get; set; }
    public decimal M_T_Radf_K { get; set; }
    public decimal D_T_Radf_K { get; set; }
    public decimal MA_Radf_K { get; set; }
    public decimal Mbg_MA_Radf_K { get; set; }
    public decimal AV_Radf_K { get; set; }
    public decimal Mbg_AV_Radf_K { get; set; }
    public decimal V_Asl { get; set; }
    public decimal V_Fare { get; set; } = 0;
    public decimal M_KHLS { get; set; }
    public long ID_tbl_TarafHesab_Moj_Rdf { get; set; }
    public decimal Pors_Dr_Rdf { get; set; }
    public decimal Pors_Mbg_Rdf { get; set; }
    public decimal ID_tbl_F { get; set; }
    public string Type_tbl_F { get; set; }
    public int ID_tbl_SalMaly { get; set; }
}

