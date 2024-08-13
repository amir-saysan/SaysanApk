using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Commands.Partnership;

public class DeleteBranchByIdCommand : IRequest<SysResult<bool>>
{
    public long BranchId { get; set; }


    public DeleteBranchByIdCommand(long branchId) => BranchId = branchId;
}

public class DeleteBranchByIdCommandHandler : IRequestHandler<DeleteBranchByIdCommand, SysResult<bool>>
{
    private IPartnerRepository _partnerRepository;

    public DeleteBranchByIdCommandHandler(IPartnerRepository repository) => _partnerRepository = repository;


    public async Task<SysResult<bool>> Handle(DeleteBranchByIdCommand request, CancellationToken cancellationToken) => await _partnerRepository.DeleteBranchById(request.BranchId, cancellationToken);
}
