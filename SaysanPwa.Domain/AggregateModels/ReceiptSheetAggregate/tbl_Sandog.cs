using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

public class tbl_Sandog : Entity, IAggregateRoot
{
    public int ID_tbl_Sandog { get; set; }
    public string Name_Sandog { get; set; } = string.Empty;//نام صندوق
    public bool D_F_Sndg { get; set; }//صندوق پیش فرض
    public bool Alw_To_Mnus { get; set; }//مجوز منفی بودن
    public string Code_Hesab_Sarfasl_Moein_Sandog { get; set; }//ایدی سرفصل
    public string Code_Hesab_Sarfasl_Tafsil_Sandog { get; set; }//کد تفصیل حساب
    public bool State { get; set; }//وضعیت فعال /  غثیر فعال بودن
}
