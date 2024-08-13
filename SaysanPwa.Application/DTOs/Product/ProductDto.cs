namespace SaysanPwa.Application.DTOs.Product;

public class ProductDto
{
    public long ID_tbl_Kala { get; set; }
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
    public string Vz_Kala { get; set; } = null!;
    public byte[]? Pic_Kala { get; set; }
    public string? Pic_Path { get; set; }
    public int ID_tbl_A_P { get; set; }
    public string ID_tbl_AnbarHa { get; set; } = null!;
    public bool St_Kala { get; set; }
    public string Ful_Bach_Nam { get; set; } = string.Empty;
}