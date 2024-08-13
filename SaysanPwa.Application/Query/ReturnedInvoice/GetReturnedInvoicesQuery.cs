using SaysanPwa.Application.DTOs.ReturnedInvoice;
using AutoMapper;
using MediatR;
using SaysanPwa.Domain.AggregateModels.Factor;

namespace SaysanPwa.Application.Query.ReturnedInvoice;

public class GetReturnedInvoiceQuery : IRequest<List<ReturnedInvoiceDto>>
{
    public int Offset { get; set; }
    public int FiscalYear { get; set; }
    public int UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public GetReturnedInvoiceQuery(int fiscalYear, int userId, string Date1, string Date2,int offset = 0)
    {
        Offset = offset;
        FiscalYear = fiscalYear;
        UserId = userId;
        From = Date1;
        To = Date2;
    }
}

public class GetReturnedInvoiceQueryHanelr : IRequestHandler<GetReturnedInvoiceQuery, List<ReturnedInvoiceDto>>
{
    private readonly IFactorRepository _repo;
    private readonly IMapper _mapper;

    public GetReturnedInvoiceQueryHanelr(IFactorRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<ReturnedInvoiceDto>> Handle(GetReturnedInvoiceQuery request, CancellationToken cancellationToken)
    {
        List<Domain.AggregateModels.Factor.ReturnedInvoice> ReturnedInvoice = await _repo.GetReturnedInvoices(request.FiscalYear, request.UserId, request.From, request.To);
        List<ReturnedInvoiceDto> result = _mapper.Map<List<ReturnedInvoiceDto>>(ReturnedInvoice);
        return result;
    }
}
