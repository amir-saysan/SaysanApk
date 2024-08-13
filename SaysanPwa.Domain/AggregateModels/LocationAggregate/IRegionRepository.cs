using SaysanPwa.Domain.SeedWorker;


namespace SaysanPwa.Domain.AggregateModels.LocationAggregate
{
    public interface IRegionRepository : IRepository<Region>
    {
        public Task<SysResult<IEnumerable<Region>>> GetRegionsAsync(CancellationToken cancellationToken = default);
        public Task<SysResult<string>> GetRegionNameById(int regionId);
    }
}
