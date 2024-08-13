using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.MarketerAggregate;

public interface IMarketerRepository : IRepository<Marketer>
{
    public Task<SysResult<long>> GetMarketerIdByPartnerIdAsync(long partnerId, CancellationToken cancellationToken = default);
}