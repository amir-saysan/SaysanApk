using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.Factor;

public class Factor : Entity, IAggregateRoot
{
    public long ID_tbl_PF { get; set; }
    public long N_PF { get; set; }//شماره فاکتور
    public string Dt_PF { get; set; } = string.Empty;//تاریخ فاکتور
    public decimal J_PF { get; set; }//جمع فاکتور
    public decimal JAv_PF { get; set; }//جمع عوارض
    public decimal JM_PF { get; set; }//جمع مالیات
	public decimal J_Tedad_Asl_PF { get; set; }//جمع تعداد فاکتور
    public decimal Tkh_A_PF { get; set; }//تحفیف فاکتور
    //public decimal JM_F { get; set; }//مالیات
    public decimal J_Hz_PF { get; set; }//هزینه
    public string Dc_PF { get; set; } = string.Empty;//شرح
    public string Name_TarafHesab { get; set; }//نام مشتری
    public string Code_TarafHesab { get; set; }//کد مشتری
    public string ChelPhone_TarafHesab { get; set; }//تلفن مشتری
    public string Name_Bzy { get; set; }//نام بازاریاب
    public string Name_Kala { get; set; }//نام کالا
    public string BarCode_Kala { get; set; }//بارکد کالا
    public long Code_Kala { get; set; }//کد کالا
    public string Shenase_Kala { get; set; }//شناسه مالیاتی کالا
    public string Vahede_Name { get; set; }//واحد کالا
    public string Name_Anbar { get; set; }//نام انبار
    public string Code_Anbar { get; set; }//کد انبار
    public decimal Tedad { get; set; }//تعداد
    public decimal Fi { get; set; }//فی
    public decimal Mablagh { get; set; }//مبلغ کل
    public string Name_Branch { get; set; }//نام شعبه
                                           /////////////////////////////////////////////////////////////////
    //                                       //

    ////فروش
    //public long ID_tbl_FF { get; set; }
    //public long N_F { get; set; }//شماره فاکتور
    //public string Dt_F { get; set; } = string.Empty;//تاریخ فاکتور
    //public decimal J_F { get; set; }//جمع فاکتور
    //public decimal Tkh_A_F { get; set; }//تحفیف فاکتور
    //public decimal JM_F { get; set; }//مالیات
    //public decimal J_Hz_F { get; set; }//هزینه
    //public string Dc_F { get; set; } = string.Empty;//شرح
    //public string Name_TarafHesab { get; set; }//نام مشتری
    //public string Code_TarafHesab { get; set; }//کد مشتری
    //public string ChelPhone_TarafHesab { get; set; }//تلفن مشتری
    //public string Name_Bzy { get; set; }//نام بازاریاب
    //public string Name_Kala { get; set; }//نام کالا
    //public string BarCode_Kala { get; set; }//بارکد کالا
    //public long Code_Kala { get; set; }//کد کالا
    //public string Shenase_Kala { get; set; }//شناسه مالیاتی کالا
    //public string Vahede_Name { get; set; }//واحد کالا
    //public string Name_Anbar { get; set; }//نام انبار
    //public string Code_Anbar { get; set; }//کد انبار
    //public decimal Tedad { get; set; }//تعداد
    //public decimal Fi { get; set; }//فی
    //public decimal Mablagh { get; set; }//مبلغ کل
    //public string Name_Branch { get; set; }//نام شعبه

    ////////////////////////////////////
    /////
    ////برگشت از فروش
    //public long ID_tbl_FBB { get; set; }
    //public long N_FBB { get; set; }//شماره فاکتور
    //public string Dt_FBB { get; set; } = string.Empty;//تاریخ فاکتور
    //public decimal J_FBB { get; set; }//جمع فاکتور
    //public decimal Tkh_A_FBB { get; set; }//تحفیف فاکتور
    //public decimal JM_F { get; set; }//مالیات
    //public decimal J_Hz_FBB { get; set; }//هزینه
    //public string Dc_FBB { get; set; } = string.Empty;//شرح
    //public string Name_TarafHesab { get; set; }//نام مشتری
    //public string Code_TarafHesab { get; set; }//کد مشتری
    //public string ChelPhone_TarafHesab { get; set; }//تلفن مشتری
    //public string Name_Bzy { get; set; }//نام بازاریاب
    //public string Name_Kala { get; set; }//نام کالا
    //public string BarCode_Kala { get; set; }//بارکد کالا
    //public long Code_Kala { get; set; }//کد کالا
    //public string Shenase_Kala { get; set; }//شناسه مالیاتی کالا
    //public string Vahede_Name { get; set; }//واحد کالا
    //public string Name_Anbar { get; set; }//نام انبار
    //public string Code_Anbar { get; set; }//کد انبار
    //public decimal Tedad { get; set; }//تعداد
    //public decimal Fi { get; set; }//فی
    //public decimal Mablagh { get; set; }//مبلغ کل
    //public string Name_Branch { get; set; }//نام شعبه



    //// خدمت فروش
    //public long ID_tbl_FF_KHed { get; set; }
    //public long N_FF_KHed { get; set; }//شماره فاکتور
    //public string Dt_FF_KHed { get; set; } = string.Empty;//تاریخ فاکتور
    //public decimal J_FF_KHed { get; set; }//جمع فاکتور
    //public decimal Tkh_A_FF_KHed { get; set; }//تحفیف فاکتور
    //public decimal JM_F { get; set; }//مالیات
    //public decimal J_Hz_FF_KHed { get; set; }//هزینه
    //public string Dc_FF_KHed { get; set; } = string.Empty;//شرح
    //public string Name_TarafHesab { get; set; }//نام مشتری
    //public string Code_TarafHesab { get; set; }//کد مشتری
    //public string ChelPhone_TarafHesab { get; set; }//تلفن مشتری
    //public string Name_TarafHesab_Moj_Rdf { get; set; }//نام مجری
    //public string Code_TarafHesab_Moj_Rdf { get; set; }//کد مجری
    //public string Name_Bzy { get; set; }//نام بازاریاب
    //public string Name_Khedmat { get; set; }//نام خدمت
    //public long Cde_Khedmat { get; set; }//کد خدمت
    //public string Shenase_Khedmat { get; set; }//شناسه مالیاتی خدمت
    //public decimal Tedad { get; set; }//تعداد
    //public decimal Fi { get; set; }//فی
    //public decimal Mablagh { get; set; }//مبلغ کل
    //public string Name_Branch { get; set; }//نام شعبه
}

