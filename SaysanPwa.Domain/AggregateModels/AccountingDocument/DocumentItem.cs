namespace SaysanPwa.Domain.AggregateModels.AccountingDocument;

internal class DocumentItem
{
    public string Name_Hst_S2 { get; set; }
    public string Code_Hst_S2 { get; set; }
    public string Name_HsMt_S2 { get; set; }
    public string Code_HsMt_S2 { get; set; }
    public string Code_HsKt_S2 { get; set; }
    public string Code_HsGt_S2 { get; set; }
    public string Name_tbl_Self_Hesab_S2 { get; set; }
    public long ID_tbl_Self_Hesab_S2 { get; set; }
    public string Desc_S2 { get; set; }
    public decimal Bedehkar_S2 { get; set; }
    public decimal Bestankar_S2 { get; set; }
    public int ID_tbl_Arz1 { get; set; } = 0;
    public decimal Mbg_Arz1 { get; set; } = 0;
    public decimal Tmbg { get; set; } = 0;
    public int ID_tbl_Arz_Mabna { get; set; } = 0;
    public decimal Mbg_Arz_Mabna { get; set; }
    public decimal Mbg_Arz_Riyal { get; set; }
    public long ID_tbl_S1 { get; set; }
    public string Create_By_Frm_P_s2 { get; set; }
    public string Created_By_Tb_N_S2 { get; set; }
    public string Created_By_Tb_I_S2 { get; set; }
    public bool Comb_S2 { get; set; } = false;
    public bool Trans_Frm_Old_V_Ch { get; set; } = false;
    public int ID_tbl_SalMaly { get; set; }

}

