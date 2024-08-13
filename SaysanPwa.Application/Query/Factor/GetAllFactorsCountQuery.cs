using SaysanPwa.Application.DTOs.Factor;
using AutoMapper;
using MediatR;
using SaysanPwa.Domain.AggregateModels.Factor;

namespace SaysanPwa.Application.Query.Factor;

public class GetFactorQuery : IRequest<List<FactorDto>>
{
    public int Offset { get; set; }
    public int FiscalYear { get; set; }
    public int UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public GetFactorQuery(int fiscalYear , int userId, string Date1, string Date2)
    {
        FiscalYear = fiscalYear;
        UserId = userId;
        From = Date1;
        To = Date2;
    }
}

public class GetFactorQueryHanelr : IRequestHandler<GetFactorQuery, List<FactorDto>>
{
    private readonly IFactorRepository _repo;
    private readonly IMapper _mapper;

    public GetFactorQueryHanelr(IFactorRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<FactorDto>> Handle(GetFactorQuery request, CancellationToken cancellationToken)
    {
        List<Domain.AggregateModels.Factor.Factor> Factor = await _repo.GetFactors(request.FiscalYear, request.UserId, request.From, request.To);
        List<FactorDto> result = _mapper.Map<List<FactorDto>>(Factor);
        return result;
    }
}
