using MediatR;
using Fs = SaysanPwa.Domain.AggregateModels.FiscalYear.FiscalYear;
using SaysanPwa.Domain.AggregateModels.FiscalYear;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.FiscalYear;

public class GetLastFiscalYearIdQuery : IRequest<SysResult<Fs>> { };

public class GetLastFiscalYearIdQueryHandler : IRequestHandler<GetLastFiscalYearIdQuery, SysResult<Fs>>
{
    private readonly IFiscalYearRepository _repostiroy;

    public GetLastFiscalYearIdQueryHandler(IFiscalYearRepository repository)
    {
        _repostiroy = repository;
    }
    public async Task<SysResult<Fs>> Handle(GetLastFiscalYearIdQuery request, CancellationToken cancellationToken) =>
        await _repostiroy.LastFiscalYearAsync(cancellationToken);
}