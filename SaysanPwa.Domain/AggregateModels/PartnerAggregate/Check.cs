namespace SaysanPwa.Domain.AggregateModels.PartnerAggregate;

public class Check
{
    /// <summary>
    /// وضعیت انگلیسی چک
    /// </summary>
    public string rdoEnglish_V_Ch { get; set; } = string.Empty;

    /// <summary>
    /// وضعیت فارسی چک
    /// </summary>
    public string L_rdoPersian_V_Ch { get; set; } = string.Empty;

    /// <summary>
    /// تاریخ سر رسید چک
    /// </summary>
    public string Dt_S_Chek { get; set; } = string.Empty;

    /// <summary>
    /// تاریخ دریافت چک
    /// </summary>
    public string Dt_D_Chek { get; set; } = string.Empty;

    /// <summary>
    /// مبلغ چک
    /// </summary>
    public decimal Mablagh_Chek { get; set; }

    /// <summary>
    /// پشت نمره چک
    /// </summary>
    public int PN { get; set; }

    /// <summary>
    /// شماره چک
    /// </summary>
    public string Shomare_Chek { get; set; } = string.Empty;

    /// <summary>
    /// شناسه طرفحساب
    /// </summary>
    public long ID_tbl_TarafHesab { get; set; }

    public long ID_tbl_Daryaft_Chek { get; set; }

    /// <summary>
    /// شرح چک
    /// </summary>
    public string Desc_Chek { get; set; } = string.Empty;

    /// <summary>
    /// شماره حساب چک
    /// </summary>
    public string Number_Hesab_Chek { get; set; } = string.Empty;

    /// <summary>
    /// شماره رسید دریافت
    /// </summary>
    public long ID_tbl_DA { get; set; }

    /// <summary>
    /// تاریخ رسید دریافت
    /// </summary>
    public string Dt_DA { get; set; } = string.Empty;

    /// <summary>
    /// نوع دریافت از چه کسی
    /// </summary>
    public string Typ_DA { get; set; } = string.Empty;

    /// <summary>
    /// نام بانک و شعبه چک دریافتی
    /// </summary>
    public string Name_Bank_Banch { get; set; } = string.Empty;

    /// <summary>
    /// کاربر ثبت کننده
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// نام مشتری
    /// </summary>
    public string Name_TarafHesab { get; set; } = string.Empty;
}