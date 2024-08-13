using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.UserAggregate;

public interface IUserRepository : IRepository<User>
{
    public Task<SysResult<User>> GetUserByUserNameAsync(string username, CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<bool> GetUserPermission(string typPermission, int userId);
}
