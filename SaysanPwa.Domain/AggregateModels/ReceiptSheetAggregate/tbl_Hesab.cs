using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

public class tbl_Hesab : Entity, IAggregateRoot
{
    public int ID_tbl_Hesab { get; set; }
    public string Number_Hesab { get; set; }//شماره حساب
    public string? SH_K_Hesab { get; set; }//شماره کارت
    public string? Shb_Hesab { get; set; }//شماره شبا
    public string NameSaheb_Hesab { get; set; }//نام صاحب جساب
    public bool Alw_To_Mnus { get; set; }//مجوز منفی بودن
    public string? Desc_Hesab { get; set; }//توضیحات
    public long Parent_ID_tbl_Sarfasl { get; set; }//ایدی سرفصل
    public string Code_Hesab_Sarfasl_Tafsil_Hesab { get; set; }//کد تفصیل حساب
    public bool State { get; set; }//وضعیت فعال /  غثیر فعال بودن
}
