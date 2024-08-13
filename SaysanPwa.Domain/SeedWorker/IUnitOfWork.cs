namespace SaysanPwa.Domain.SeedWorker;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
