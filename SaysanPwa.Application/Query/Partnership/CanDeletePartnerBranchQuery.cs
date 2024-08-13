using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Partnership;

public class CanDeletePartnerBranchQuery : IRequest<SysResult<string>>
{
    public CanDeletePartnerBranchQuery(int branchId, int partnerId)
    {
        BranchId = branchId;
        PartnerId = partnerId;
    }

    public int BranchId { get; set; }
    public int PartnerId { get; set; }

}

public class CanDeletePartnerBranchQueryHandler : IRequestHandler<CanDeletePartnerBranchQuery, SysResult<string>>
{
    private readonly IPartnerRepository _repository;

    public CanDeletePartnerBranchQueryHandler(IPartnerRepository repository) => _repository = repository;

    public async Task<SysResult<string>> Handle(CanDeletePartnerBranchQuery request, CancellationToken cancellationToken) =>
        await _repository.CanDeleteBranch(request.BranchId, request.PartnerId);
}
