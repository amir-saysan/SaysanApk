using SaysanPwa.Domain.AggregateModels.Company;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;

namespace SaysanPwa.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly IDbManager _dbManager;

    public CompanyRepository(IDbManager dbManager)
    {
        _dbManager = dbManager;
    }

    public async Task<SysResult<Company>> CurrentCompanyInformation(CancellationToken cancellationToken = default)
    {
        try
        {
            return new(await _dbManager.CallSingleRowProcedureAsync<Company>("Apk_Proc_GetInformation_Compony"));
        }
        catch (Exception ex)
        {
            return new(null!, false, new() { ex.Message });
        }
    }
}