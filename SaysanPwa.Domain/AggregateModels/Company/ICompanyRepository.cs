using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.Company;

public interface ICompanyRepository : IRepository<Company>
{
    public Task<SysResult<Company>> CurrentCompanyInformation(CancellationToken cancellationToken = default);
}
