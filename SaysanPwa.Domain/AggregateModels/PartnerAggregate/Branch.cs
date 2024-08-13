using SaysanPwa.Domain.SeedWorker;


namespace SaysanPwa.Domain.AggregateModels.PartnerAggregate;

public class Branch : Entity
{
    public long ID_tbl_Partner_Branch { get; set; }
    public long ID_tbl_TarafHesab { get; set; }
    public string Name_responsible { get; set; } = null!;
    public string Name_Branch { get; set; } = null!;
    public bool State_Branch { get; set; }
    public string? Fax { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int? ID_tbl_Ostan { get; set; }
    public int? ID_tbl_SharOstan { get; set; }
    public string? CodePosti { get; set; }
    public string? Tell { get; set; }
    public string? Address { get; set; }
    public string? Location_Branch { get; set; }

}

