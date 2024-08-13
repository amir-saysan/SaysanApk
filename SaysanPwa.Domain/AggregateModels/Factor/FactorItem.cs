
namespace SaysanPwa.Domain.AggregateModels.Factor;

public class FactorItem
{
    public long ID_tbl_Kala { get; set; }
    public int ID_tbl_Anbar { get; set; }
    public int ID_tblVahede_Kala { get; set; }
    public decimal Tedad { get; set; }
    public decimal Fi_Bed_Haz { get; set; }
    public decimal Fi_Bed_Haz_Az { get; set; } = 0.0M;
    public decimal Fi_Ba_Takh { get; set; }
    public decimal Fi_Ba_Takh_Az { get; set; } = 0.0M;
    public decimal Fi_Ba_Haz { get; set; }
    public decimal Fi_Ba_Haz_Az { get; set; } = 0.0M;
    public decimal M_T_Radf_K { get; set; }
    public decimal M_T_Radf_K_Az { get; set; } = 0.0M;
    public decimal D_T_Radf_K { get; set; } = 0.0M;
    public decimal MA_Radf_K { get; set; }
    public decimal MA_Radf_K_Az { get; set; } = 0.0M;
    public decimal AV_Radf_K { get; set; } = 0.0M;
    public decimal AV_Radf_K_Az { get; set; } = 0.0M;
    public decimal M_AV_Radf_K { get; set; }
    public decimal M_AV_Radf_K_Az { get; set; } = 0.0M;
    public decimal V_Asl { get; set; }
    public decimal V_Fare { get; set; }
    public decimal M_KHLS { get; set; }
    public decimal M_KHLS_Az { get; set; } = 0.0M;
    public decimal Tedad_Sadere { get; set; }
    public decimal Fi_Sadere { get; set; }
    public decimal Mablagh_Sadere { get; set; }
    public decimal Mnd_Megdar { get; set; }
    public decimal Mnd_Fi { get; set; }
    public decimal Mnd_Mablagh { get; set; }
    public bool jj { get; set; } = false;
    public long ID_tbl_F { get; set; }
    public string Type_tbl_F { get; set; }
    public bool Spnd_tbl_F { get; set; } = false;
    public long ID_tbl_F_Aglm_Ref_Sofa_Tld { get; set; } = 0;
    public int ID_tbl_SalMaly { get; set; }
}

