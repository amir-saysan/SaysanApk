namespace SaysanPwa.Application.DTOs.Factor;

public record AddFactorItemDto
{
    public long ID_tbl_Kala { get; set; }
    public int ID_tbl_Anbar { get; set; }
    public int ID_tblVahede_Kala { get; set; }
    public decimal Tedad { get; set; }//تعداد
    public decimal Fi_Bed_Haz { get; set; }//فی
    public decimal Fi_Ba_Takh { get; set; }// fi bad takfif
    public decimal Fi_Ba_Haz { get; set; }// fi bad takfif
    public decimal M_T_Radf_K { get; set; } = 0.00M;//مبلف تخفیف ردیف
    public decimal D_T_Radf_K { get; set; } = 0.00M;//درصد تخفیف ردیف
    public decimal MA_Radf_K { get; set; }//درصد مالیات ردیف فاکتور
    public decimal AV_Radf_K { get; set; } //درصد عوارض کالا
    public decimal M_AV_Radf_K { get; set; }//MA_Radf_K //میلغ مالیات ردیف فامکتور
    public decimal V_Asl { get; set; }//تعداد
    public decimal V_Fare { get; set; } = 0.00M;
    public decimal M_KHLS { get; set; }//مبلغ نهایی ردیف فاکتور
    public decimal Tedad_Sadere { get; set; }//تعداد
    public decimal Fi_Sadere { get; set; }//فی
    public decimal Mablagh_Sadere { get; set; }//مبلغ کل = تعداد  * فی
    public decimal Mnd_Megdar { get; set; } = 0.00M;
    public decimal Mnd_Fi { get; set; } = 0.00M;
    public decimal Mnd_Mablagh { get; set; } = 0.00M;
    public bool jj { get; set; } = false;
    //public long ID_tbl_F { get; set; }
    public string Type_tbl_F { get; set; } = "";//Pish FactorForosh=tbl_PF    FactorForosh=tbl_FF
    public int ID_tbl_SalMaly { get; set; } = 1;
}
