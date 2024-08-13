namespace SaysanPwa.Domain.AggregateModels.Factor;

public class GetPartnersForFactorViewModel
{
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Partner_Branch { get; set; }
    public string? Name_TarafHesab { get; set; }
    public string? Code_TarafHesab { get; set; }
    public string? Name_Branch { get; set; }
    public string? Brnach_Asli_Farei { get; set; }
    public bool Karmand_TarafHesab { get; set; }
    public bool Kharidar_TarafHesab { get; set; }
}
