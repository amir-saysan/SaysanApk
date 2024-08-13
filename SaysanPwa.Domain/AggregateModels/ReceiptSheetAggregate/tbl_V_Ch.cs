using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

public class tbl_V_Ch : Entity, IAggregateRoot
{
    public long ID_tbl_Daryaft_Chek { get; set; }
    public string Code_Hesab_Sarfasl_Tafsil_Daryaft_Chek { get; set; }
    public string Code_Hesab_Sarfasl_Moein_Daryaft_Chek { get; set; }
    public string rdoEnglish_V_Ch { get; set; }
    public string rdoPersian_V_Ch { get; set; }
    public string Date_V_Ch { get; set; }
    public string Time_V_Ch { get; set; }
    public int ID_tbl_Users { get; set; }
    public string Code_Hesab_Sarfasl_Tafsil_Kh_Chek { get; set; }
    public string Name_Hesab_Sarfasl_Tafsil_Kh_Chek { get; set; }
    public string Shomare_Vagozari_Kh_Chek { get; set; }
    public bool Use_AvalD_V_Ch { get; set; }
    public bool Trans_Frm_Old { get; set; }
    public long ID_tbl_S1 { get; set; }
}
