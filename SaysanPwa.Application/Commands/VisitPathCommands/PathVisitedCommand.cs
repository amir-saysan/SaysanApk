using MediatR;
using SaysanPwa.Domain.AggregateModels.VisitPathAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Commands.VisitPathCommands
{
    public record PathVisitedCommand(long ID_tbl_Bzy, long ID_tbl_TarafHesab, long ID_tbl_Partner_Branch, string description = "") : IRequest<SysResult<bool>>;

    public class PathVisitedCommandHandler : IRequestHandler<PathVisitedCommand, SysResult<bool>>
    {
        private readonly IVisitPathRepository _visitPathRepository;

        public PathVisitedCommandHandler(IVisitPathRepository visitPathRepository)
        {
            _visitPathRepository = visitPathRepository;
        }

        public async Task<SysResult<bool>> Handle(PathVisitedCommand request, CancellationToken cancellationToken)
        {
            return await _visitPathRepository.PathVisitedAsync(request.ID_tbl_Bzy, request.ID_tbl_TarafHesab, request.ID_tbl_Partner_Branch, request.description);
        }
    }
}
