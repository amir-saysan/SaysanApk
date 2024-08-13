using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ProductAggregate;

public class Product : Entity, IAggregateRoot
{
    public long ID_tbl_Kala { get; set; }
    public int ID_tbl_G_Tee { get; set; }
    public string Name_Kala { get; set; } = string.Empty;
    public long Code_Kala { get; set; }    
    public string? Shenase_Kala { get; set; }
    public string? Desc_Shenase_Kala { get; set; }
    public string? BarCode_Kala { get; set; }
    public string? BarCode_Kala2 { get; set; }
    public string? BarCode_Kala3 { get; set; }
    public string? BarCode_Kala4 { get; set; }
    public bool Alw_To_Mnus { get; set; }
    public int Darayi_Kala { get; set; }
    public int T_Hadg_Kala { get; set; }
    public int T_Hadks_Kala { get; set; }
    public string Tp_G_F_Kala { get; set; } = string.Empty;
    public string Tp_G_E_Kala { get; set; } = string.Empty;
    public decimal Price_Forosh_Kala { get; set; }
    public decimal Price_Kharid_Kala { get; set; }
    public decimal Price_Forosh_Kala_Farei { get; set; }
    public decimal Price_Kharid_Kala_Farei { get; set; }
    public string? Model_Kala { get; set; }
    public string? Sazande_Kala { get; set; }
    public decimal D_Av_Kala { get; set; }
    public decimal D_Ma_Kala { get; set; }
    public bool MO_Az_KH_Kala { get; set; }
    public bool MO_Az_FO_Kala { get; set; }
    public bool T_KH_S_F { get; set; }
    public bool Fo_Payn_Kh_No { get; set; }
    public string D_S_Kala { get; set; } = string.Empty;
    public string D_E_Kala { get; set; } = string.Empty;
    public string Fanni_Kala { get; set; } = string.Empty;
    public string Descript_Kala { get; set; } = string.Empty;
    public string Vz_Kala { get; set; } = null!;
    public string M_Grar_Kala { get; set; } = string.Empty;
    public byte[]? Pic_Kala { get; set; }
    public int ID_tbl_A_P { get; set; }
    public decimal Avl_Tedad_Kala { get; set; }
    public decimal Avl_Fii_Kala { get; set; }
    public int Avl_ID_tbl_Anbar { get; set; }
    public string ID_tbl_AnbarHa { get; set; } = string.Empty;
    public bool St_Kala { get; set; }
    public int ID_tbl_Users { get; set; }
    public string Makan_Kala { get; set; } = string.Empty;
    public string Ful_Bach_Nam { get; set; } = string.Empty;
}

