namespace SaysanPwa.Domain.AggregateModels.ProductAggregate;

public class SmallOfGoods
{
    public decimal Tedad { get; set; }
    public decimal Fi_Bed_Haz { get; set; }
    public decimal M_Kol { get; set; }
    public decimal M_T_Radf_K { get; set; }
    public decimal Fi_Ba_Haz { get; set; }
    public decimal M_AV_Radf_K { get; set; }
    public decimal M_KHLS { get; set; }
    public string Vahede_Name { get; set; } = string.Empty;
    public long Code_TarafHesab { get; set; }
    public long N_F { get; set; }
    public string Dt_F { get; set; } = string.Empty;
    public string Type_Factor { get; set; } = string.Empty;
}
