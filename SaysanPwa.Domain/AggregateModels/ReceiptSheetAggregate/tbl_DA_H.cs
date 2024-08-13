using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

public class tbl_DA_H : Entity, IAggregateRoot
{
    public long ID_tbl_DA_H { get; set; }
    public string? N_H { get; set; }
    public string? Date_H { get; set; }
    public decimal Mablag_DA_H { get; set; }
    public decimal? Mablag_DA_H_Az { get; set; }
    public int ID_tbl_Hesab { get; set; }
    public string? DC_DA_H { get; set; }
    public long ID_tbl_DA { get; set; }
    public int ID_tbl_SalMaly { get; set; }
}
