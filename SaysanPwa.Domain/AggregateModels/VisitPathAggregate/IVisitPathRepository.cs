using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.VisitPathAggregate
{
    public interface IVisitPathRepository : IRepository<VisitPath>
    {
        Task<SysResult<IEnumerable<GetVisitPathForUserModel>>> GetPathesForUser(long idBazaryab, string date, CancellationToken cancellationToken = default);
        Task<SysResult<bool>> PathVisitedAsync(long ID_tbl_Bzy, long ID_tbl_TarafHesab, long ID_tbl_Partner_Branch, string description, CancellationToken cancellationToken = default);
    }
}
