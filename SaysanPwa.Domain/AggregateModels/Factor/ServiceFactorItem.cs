namespace SaysanPwa.Domain.AggregateModels.Factor;

public class ServiceFactorItem
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