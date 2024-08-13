using SaysanPwa.Domain.AggregateModels.LocationAggregate;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;

namespace SaysanPwa.Infrastructure.Repositories;


public class RegionRepository : IRegionRepository
{
    private readonly IDbManager _dbManager;

    public RegionRepository(IDbManager dbManager) => _dbManager = dbManager;

    public async Task<SysResult<IEnumerable<Region>>> GetRegionsAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<Region> regions = await _dbManager.CallProcedureAsync<Region>("Apk_Proc_GetRegions", cancellationToken);
        return new(regions);
    }

    public async Task<SysResult<string>> GetRegionNameById(int regionId)
    {
        try
        {
            string name = await _dbManager.CallScalarProcedureWithParametersAsync<string>("Apk_Proc_GetRegions", new
            {
                ID_tbl_Ostan = regionId
            });

            return new(name!);
        }
        catch (Exception ex)
        {
            return new(string.Empty, false, new() { ex.Message });
        }
    }
}