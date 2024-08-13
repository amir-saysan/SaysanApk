using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.VisitPathAggregate
{
    public class VisitPath : Entity, IAggregateRoot
    {
        public long ID_tbl_Aglm_ToVisit_Branch_Per_Partner_Daily { get; set; }
        public long ID_tbl_Bzy { get; set; }
        public long ID_tbl_TarafHesab { get; set; }
        public long ID_tbl_Partner_Branch { get; set; }
        public string? Date_Visited { get; set; }
        public string? Time_Visited { get; set; }
        public string? Description_Visited { get; set; }
    }
}
