using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.JobAggregate
{
    public class Job : Entity, IAggregateRoot
    {
        public int ID_tbl_Job { get; set; }
        public string Title_Job { get; set; } = null!;
    }
}
