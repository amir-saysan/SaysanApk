using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.FiscalYear;

public interface IFiscalYearRepository : IRepository<FiscalYear>
{
    public Task<SysResult<FiscalYear>> LastFiscalYearAsync(CancellationToken cancellationToken = default);
}
