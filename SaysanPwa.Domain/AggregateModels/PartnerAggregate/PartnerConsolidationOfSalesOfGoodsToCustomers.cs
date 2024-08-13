namespace SaysanPwa.Domain.AggregateModels.PartnerAggregate;

public class PartnerConsolidationOfSalesOfGoodsToCustomers
{
    public long ID_tbl_TarafHesab { get; set; }
	public string Name_TarafHesab { get; set; } = string.Empty;
    public long Code_TarafHesab { get; set; }
	public string Name_Kala { get; set; } = string.Empty;
    public long Code_Kala { get; set; }
	public string Shenase_Kala { get; set; } = string.Empty;
    public decimal Tedad { get; set; }
    public decimal M_Kol { get; set; }
    public decimal M_T_Radf_K { get; set; }
    public decimal M_AV_Radf_K { get; set; }
    public decimal M_KHLS { get; set; }
    public string? Name_Branch { get; set; }
    public string? Name_responsible { get; set; }
}
