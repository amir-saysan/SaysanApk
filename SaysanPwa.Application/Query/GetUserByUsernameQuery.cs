using MediatR;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query;

public class GetUserByUsernameQuery : IRequest<SysResult<User>>
{
    public string Username { get; set; } = string.Empty;

    public GetUserByUsernameQuery(string username) => (Username) = (username);
}

public class GetUserByNameQueryHandler : IRequestHandler<GetUserByUsernameQuery, SysResult<User>>
{
    private readonly IUserRepository _repository;

    public GetUserByNameQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }


    public async Task<SysResult<User>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken) =>
        await _repository.GetUserByUserNameAsync(request.Username, cancellationToken);
}