using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;
using System.IO.Compression;

namespace SaysanPwa.Domain.AggregateModels.JobAggregate;

public interface IJobRepository : IRepository<Job>
{
    Task<SysResult<IEnumerable<Job>>> GetAllJobs(CancellationToken cancellationToken = default);

    public Task<SysResult<bool>> AddNewJob(string title, CancellationToken cancellationToken = default);
}
