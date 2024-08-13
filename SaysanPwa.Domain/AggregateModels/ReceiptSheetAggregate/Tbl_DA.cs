using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

public class Tbl_DA : Entity, IAggregateRoot
{
    public long ID_tbl_DA { get; set; }
    public long ID_tbl_TarafHesab { get; set; }
    public string Typ_DA { get; set; } = string.Empty;
    public string? Code_HsMt_S2_DA { get; set; }
    public string? Type_Tafsir_In_Sayer { get; set; }
    public int? ID_tbl_Arz { get; set; }
    public decimal? Mbg_Arz { get; set; }
    public string Dt_DA { get; set; } = string.Empty;
    public string Dt_C_DA { get; set; } = string.Empty;
    public string Tm_C_DA { get; set; } = string.Empty;
    public string? Dc_DA { get; set; }
    public decimal? M_to_S { get; set; }
    public decimal? M_to_S_Az { get; set; }
    public int? ID_tbl_Sandog { get; set; }
    public string? Dat_To_S { get; set; }
    public string? N_R_S { get; set; }
    public string? DC_To_S { get; set; }
    public decimal? J_DA_Ch { get; set; }
    public decimal? J_DA_K { get; set; }
    public decimal? J_DA_H { get; set; }
    public decimal? J_DA_H_Az { get; set; }
    public decimal? J_DA_Sayer { get; set; }
    public decimal? J_DA_Sayer_Az { get; set; }
    public decimal? J_DA { get; set; }
    public decimal? J_DA_Az { get; set; }
    public bool? Use_AvalD { get; set; }
    public string C_B_frm { get; set; } = string.Empty;
    public long? ID_tbl_S1 { get; set; }
    public bool Trans_Frm_Old_DA { get; set; }
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }
    public string? Dt_Up { get; set; }
    public string? Tm_Up { get; set; }
    public int? ID_tbl_Users_Up { get; set; }
}
