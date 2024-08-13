using MediatR;
using cmp = SaysanPwa.Domain.AggregateModels.Company;
using SaysanPwa.Domain.SeedWorker;
using SaysanPwa.Domain.AggregateModels.Company;

namespace SaysanPwa.Application.Query.Company;

public class GetCurrentCompanyQuery : IRequest<SysResult<cmp::Company>>
{

}

public class GetCurrentCompanyQueryHandler : IRequestHandler<GetCurrentCompanyQuery, SysResult<cmp::Company>>
{
    private readonly ICompanyRepository _repository;

    public GetCurrentCompanyQueryHandler(ICompanyRepository repository) => _repository = repository;

    public async Task<SysResult<cmp.Company>> Handle(GetCurrentCompanyQuery request, CancellationToken cancellationToken) => await _repository.CurrentCompanyInformation(cancellationToken);
}
