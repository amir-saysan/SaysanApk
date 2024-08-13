using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

public class tbl_DA_K : Entity, IAggregateRoot
{
    public long ID_tbl_DA_K { get; set; }
    public string? Dat_DA_K { get; set; }
    public int ID_tbl_Hesab { get; set; }
    public decimal Mablag_DA_K { get; set; }
    public string? DC_DA_K { get; set; }
    public string? Peygiry_Nu { get; set; }
    public string? Traction_Nu { get; set; }
    public long ID_tbl_DA { get; set; }
    public int ID_tbl_SalMaly { get; set; }
}
