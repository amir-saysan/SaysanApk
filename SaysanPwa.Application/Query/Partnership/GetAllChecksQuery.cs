using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Partnership;

public class GetAllChecksQuery : IRequest<SysResult<IEnumerable<Check>>>
{
    public GetAllChecksQuery(rdoEnglish_V_Ch rdoEnglish_V_Ch, long fiscalYear, long partnerId, string? from, string? to, long offset)
    {
        RdoEnglish_V_Ch = rdoEnglish_V_Ch;
        FiscalYear = fiscalYear;
        PartnerId = partnerId;
        From = from;
        To = to;
        Offset = offset;
    }

    public rdoEnglish_V_Ch RdoEnglish_V_Ch { get; set; }
    public long FiscalYear { get; set; }
    public long PartnerId { get; set; }
    public string? From { get; set; } = null!;
    public string? To { get; set; } = null!;
    public long Offset { get; set; }
}

public class GetAllChecksQueryHandler : IRequestHandler<GetAllChecksQuery, SysResult<IEnumerable<Check>>>
{
    private readonly IPartnerRepository _repository;

    public GetAllChecksQueryHandler(IPartnerRepository repostiory) => _repository = repostiory;

    public async Task<SysResult<IEnumerable<Check>>> Handle(GetAllChecksQuery request, CancellationToken cancellationToken) =>
        await _repository.GetAllChecks(request.RdoEnglish_V_Ch, request.FiscalYear, request.PartnerId, request.From, request.To, request.Offset, cancellationToken);
}