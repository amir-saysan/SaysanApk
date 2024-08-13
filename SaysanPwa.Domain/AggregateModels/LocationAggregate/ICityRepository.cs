using SaysanPwa.Domain.SeedWorker;


namespace SaysanPwa.Domain.AggregateModels.LocationAggregate
{
    public interface ICityRepository : IRepository<City>
    {
        public Task<SysResult<IEnumerable<City>>> GetCitiesAsync(int regionId, CancellationToken cancellationToken = default);
        public Task<SysResult<string>> GetCityNameById(int cityId);
    }
}
