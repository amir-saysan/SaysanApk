using SaysanPwa.Domain.SeedWorker;


namespace SaysanPwa.Domain.AggregateModels.PartnerAggregate
{
    public class PartnerGroup : Entity, IAggregateRoot
    {
        public int ID_tbl_Group_TF { get; set; }
        public string Name_Group { get; set; } = null!;
    }
}
