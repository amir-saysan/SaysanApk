using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

public class tbl_Daryaft_Chek : Entity, IAggregateRoot
{
    public long ID_tbl_TarafHesab { get; set; }
    public string Shomare_Chek { get; set; }
    public decimal Mablagh_Chek { get; set; }
    public string? Serial_Chek { get; set; }
    public string Dt_D_Chek { get; set; }
    public string Dt_S_Chek { get; set; }
    public string? Number_Hesab_Chek { get; set; }
    public int PN { get; set; }
    public int ID_tbl_Bank { get; set; }
    public int ID_tbl_BanchBank { get; set; }
    public string? Desc_Chek { get; set; }
    public long ID_tbl_DA { get; set; }
    public string L_rdoPersian_V_Ch { get; set; }
    public bool Is_Final_Reg { get; set; }
    public bool Is_Khrj_Ch { get; set; }
    public bool Trans_Frm_Old_DCh { get; set; }
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }
    public string? Dt_Up { get; set; }
    public string? Tm_Up { get; set; }
    public int? ID_tbl_Users_Up { get; set; }
}
