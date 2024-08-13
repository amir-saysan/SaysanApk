using MediatR;
using SaysanPwa.Application.DTOs.User;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query;

public class GetAllUsersQuery : IRequest<SysResult<IEnumerable<UserDto>>>
{

}

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, SysResult<IEnumerable<UserDto>>>
{
    private IUserRepository _repository;

    
    public GetAllUsersQueryHandler(IUserRepository repository)
    {
        this._repository = repository;
    }

    public async Task<SysResult<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllUsersAsync(cancellationToken);
        return new (result.Result.Select(s => new UserDto(s.ID_tbl_Users, s.Username, s.State_User)).ToList(), result.Succeeded, result.ErrorMessages?.ToList() ?? null!);
    }
}
