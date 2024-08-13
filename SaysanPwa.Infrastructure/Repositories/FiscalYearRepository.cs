using SaysanPwa.Domain.AggregateModels.FiscalYear;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;

namespace SaysanPwa.Infrastructure.Repositories;

public class FiscalYearRepository : IFiscalYearRepository
{
    private readonly IDbManager _dbManager;

    public FiscalYearRepository(IDbManager dbManager) => _dbManager = dbManager;

    public async Task<SysResult<FiscalYear>> LastFiscalYearAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _dbManager.CallSingleRowProcedureAsync<FiscalYear>("Apk_Proc_SalMaly", cancellationToken);
            return new(response);
        }
        catch (Exception ex)
        { 
            return new(null!, false, new() { ex.Message });
        }
    }
}
