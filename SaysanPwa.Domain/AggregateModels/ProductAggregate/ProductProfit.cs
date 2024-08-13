namespace SaysanPwa.Domain.AggregateModels.ProductAggregate;

public class ProductProfit
{
    /// <summary>
    /// شماره فاکتور
    /// </summary>
    public long N_F { get; set; }

    /// <summary>
    /// تاریخ فاکتور
    /// </summary>
    public string Dt_F { get; set; } = string.Empty;

    /// <summary>
    /// تعداد
    /// </summary>
    public decimal Tedad { get; set; }

    /// <summary>
    /// فی خالص
    /// </summary>
    public decimal Fi_Khales { get; set; }

    /// <summary>
    /// مبلغ کل
    /// </summary>
    public decimal M_Koll { get; set; }

    /// <summary>
    /// فی ناخالص
    /// </summary>
    public decimal Fi_Nakhales { get; set; }

    /// <summary>
    /// مالیات عوارض
    /// </summary>
    public decimal M_AV_Radf_K { get; set; }

    /// <summary>
    /// تخفیف
    /// </summary>
    public decimal M_T_Radf_K { get; set; }

    /// <summary>
    /// مبلغ ناخالص
    /// </summary>
    public decimal M_KHLS { get; set; }

    /// <summary>
    /// سود
    /// </summary>
    public decimal Pf_F { get; set; }

    /// <summary>
    /// نام کالا
    /// </summary>
    public string Name_Kala { get; set; } = string.Empty;

    /// <summary>
    /// کد کالا
    /// </summary>
    public long Code_Kala { get; set; }

    /// <summary>
    /// گروه کالا
    /// </summary>
    public string Ful_Bach_Nam { get; set; } = string.Empty;

    /// <summary>
    /// نام مشتری
    /// </summary>
    public string Name_TarafHesab { get; set; } = string.Empty;


    /// <summary>
    /// کد مشتری
    /// </summary>
    public long Code_TarafHesab { get; set; }
}
