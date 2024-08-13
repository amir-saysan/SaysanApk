namespace SaysanPwa.Domain.AggregateModels.PartnerAggregate;

public class SellReport
{
    
    public long ID_tbl_FF { get; set; }

    /// <summary>
    /// شماره فاکتور
    /// </summary>
    public long N_F { get; set; }

    /// <summary>
    /// شرح فاکتور
    /// </summary>
    public string Dt_F { get; set; } = string.Empty;

    /// <summary>
    /// جمع فاکتور
    /// </summary>
    public decimal J_F { get; set; }

    /// <summary>
    /// تخفیف فاکتور
    /// </summary>
    public decimal Tkh_A_F { get; set; }

    /// <summary>
    /// جمع مالیات فاکتور
    /// </summary>
    public decimal JM_F { get; set; }

    /// <summary>
    /// نام مشتری
    /// </summary>
    public string Name_TarafHesab { get; set; } = string.Empty;

    /// <summary>
    /// کد مشتری
    /// </summary>
    public long Code_TarafHesab { get; set; }

    /// <summary>
    /// نام بازاریاب
    /// </summary>
    public string Name_Bzy { get; set; } = string.Empty;

    /// <summary>
    /// نام کالا
    /// </summary>
    public string Name_Kala { get; set; } = string.Empty;

    /// <summary>
    /// بارکد کالا
    /// </summary>
    public string BarCode_Kala { get; set; } = string.Empty;

    /// <summary>
    /// شناسه مالیاتی کالا
    /// </summary>
    public string Shenase_Kala { get; set; } = string.Empty;

    /// <summary>
    /// واحد کالا
    /// </summary>
    public string Vahede_Name { get; set; } = string.Empty;

    /// <summary>
    /// نام انبار
    /// </summary>
    public string Name_Anbar { get; set; } = string.Empty;

    /// <summary>
    /// کد انبار
    /// </summary>
    public string Code_Anbar { get; set; } = string.Empty;

    /// <summary>
    /// تعداد
    /// </summary>
    public decimal Tedad { get; set; }

    /// <summary>
    /// فی
    /// </summary>
    public decimal Fi { get; set; }


    /// <summary>
    /// مبلغ کل
    /// </summary>
    public decimal Mablagh { get; set; }


    /// <summary>
    /// نام شعبه
    /// </summary>
    public string Name_Branch { get; set; } = string.Empty;


    /// <summary>
    /// شماره تلفن طرف حساب
    /// </summary>
    public string ChelPhone_TarafHesab { get; set; } = string.Empty;
}
