namespace SaysanPwa.Domain.AggregateModels.Factor;

public class Services
{
    public long ID_tbl_Khedmat { get; set; }
    /// <summary>
    /// نام خدمت
    /// </summary>
    public string Name_Khedmat { get; set; } = string.Empty;
    /// <summary>
    ///     کد خدمت
    /// </summary>
    public long Cde_Khedmat { get; set; }
    /// <summary>
    /// آیدی واحد کالا
    /// </summary>
    public int ID_tblVahede_Kala { get; set; }
    /// <summary>
    /// قیمت خرید
    /// </summary>
    public decimal P_K_Khedmat { get; set; }
    /// <summary>
    /// قیمت فروش
    /// </summary>
    public decimal P_F_Khedmat { get; set; }
    /// <summary>
    /// شامل مالیات و عوارض
    /// </summary>
    public bool IS_A_M_Khedmat { get; set; }
    /// <summary>
    /// درصد عوارض
    /// </summary>
    public int A_Khedmat { get; set; }
    /// <summary>
    /// درصد مالیات
    /// </summary>
    public int M_Khedmat { get; set; }
    /// <summary>
    /// شرح
    /// </summary>
    public string Des_Khedmat { get; set; } = string.Empty;
    /// <summary>
    /// وضعیت فعال/ غیر فعال
    /// </summary>
    public bool State { get; set; }
    /// <summary>
    /// کد شناسه مالیاتی
    /// </summary>
    public string Shenase_Khedmat { get; set; } = string.Empty;
    /// <summary>
    /// شرح شناسه مالیاتی
    /// </summary>
    public string Desc_Shenase_Khedmat { get; set; } = string.Empty;

    public string Vahede_Name { get; set; }
}
