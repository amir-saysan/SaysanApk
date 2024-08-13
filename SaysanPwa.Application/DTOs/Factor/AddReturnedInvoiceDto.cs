namespace SaysanPwa.Application.DTOs.Factor;

public record AddReturnedInvoiceDto
{
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Partner_Branch { get; set; }
    public long ID_tbl_Bzy { get; set; }
    public decimal Tkh_N_FBB { get; set; }
    public decimal Tkh_A_FBB { get; set; }
    public decimal JM_FBB { get; set; }
    public decimal JAv_FBB { get; set; }
    public decimal J_F_FBB { get; set; }
    public decimal J_FBB { get; set; }
    public decimal BG_FBB { get; set; } = 0;
    public decimal J_Tedad_Asl_FBB { get; set; }
    public decimal J_Tedad_Fare_FBB { get; set; }
    public string Dc_FBB { get; set; } = "";
    public decimal Prsnt_Bzy { get; set; } = 0;
    public decimal Pdsh_Bzy { get; set; } = 0;
    public string Dt_FBB { get; set; }
    public string Dt_C_FBB { get; set; } //Dt_F;
    public string Tm_C_FBB { get; set; }
    public long ID_tbl_S1 { get; set; } = 0;
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }

    // ------------- For set fiscal year information to sale factor.

    public string FiscalYearBeginDate { get; set; } = string.Empty;
    public string FiscalYearEndDate { get; set; } = string.Empty;
    public string FiscalYearTitle { get; set; } = string.Empty;

    public IEnumerable<AddFactorItemDto> Items { get; set; } = new List<AddFactorItemDto>();
}
