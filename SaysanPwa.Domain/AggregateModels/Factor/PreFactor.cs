namespace SaysanPwa.Domain.AggregateModels.Factor;

public class PreFactor
{
    public long ID_tbl_PF { get; set; }
    public string Type_tbl_F { get; set; } = "tbl_PF";
    public string Type_Forosh { get; set; } = "Forosh_Dakheli";
    public long N_PF { get; set; }
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Partner_Branch { get; set; } = 0;
    public long ID_tbl_Bzy { get; set; }
    public int ID_tbl_Arz { get; set; } = 0;
    public decimal Mbg_Arz { get; set; } = 0;
    public decimal Tkh_N_PF { get; set; }
    public decimal Tkh_N_PF_Az { get; set; } = 0;
    public decimal Tkh_K_PF { get; set; } = 0;
    public decimal Tkh_A_PF { get; set; }
    public decimal Tkh_A_PF_Az { get; set; } = 0;
    public decimal JM_PF { get; set; }
    public decimal JM_PF_Az { get; set; } = 0;
    public decimal JAv_PF { get; set; } = 0;
    public decimal JAv_PF_Az { get; set; } = 0;
    public decimal J_Hz_PF { get; set; } = 0;
    public decimal J_Hz_PF_Az { get; set; } = 0;
    public int Typ_Haz { get; set; } = 0;
    public decimal J_PF { get; set; }
    public decimal J_PF_Az { get; set; } = 0;
    public decimal J_Tedad_Asl_PF { get; set; }
    public decimal J_Tedad_Fare_PF { get; set; } = 0;
    public string Dc_PF { get; set; } = "";
    public decimal Pf_F { get; set; }
    public decimal Prsnt_Bzy { get; set; }
    public decimal Pdsh_Bzy { get; set; }
    public string Sh_Sofa { get; set; } = "";
    public string Sh_R_Anb { get; set; } = "";
    public string Sh_F_Taraf { get; set; } = "";
    public string Dt_Sar_PF { get; set; } = "";
    public string Dt_PF { get; set; }
    public string Dt_C_PF { get; set; }
    public string Tm_C_PF { get; set; }
    public bool Conred_PF { get; set; }=false;
    public bool Haz_Shekan { get; set; } = false;
    public long ID_tbl_FF { get; set; } = 0;
    public long N_F { get; set; } = 0;
    public string C_To_F_P { get; set; } = "تبدیل نشده";
    public string IS_PR { get; set; } = "چاپ نشده";
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }
    public string Dt_Up { get; set; } = "";
    public string Tm_Up { get; set; } = "";
    public int ID_tbl_Users_Up { get; set; } = 0;
}




















