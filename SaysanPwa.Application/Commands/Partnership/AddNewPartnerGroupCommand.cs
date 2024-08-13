using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Commands.Partnership;

public class AddNewPartnerGroupCommand : IRequest<SysResult<bool>>
{
    public string GroupTitle { get; set; } = string.Empty;

    public AddNewPartnerGroupCommand(string groupTitle) => GroupTitle = groupTitle;
}

public class AddNewPartnerGroupCommandHandler : IRequestHandler<AddNewPartnerGroupCommand, SysResult<bool>>
{
    private readonly IPartnerRepository _repository;

    public AddNewPartnerGroupCommandHandler(IPartnerRepository repository) => _repository = repository;

    public async Task<SysResult<bool>> Handle(AddNewPartnerGroupCommand request, CancellationToken cancellationToken) =>
        await _repository.AddNewPartnerGroup(request.GroupTitle, cancellationToken);
}
