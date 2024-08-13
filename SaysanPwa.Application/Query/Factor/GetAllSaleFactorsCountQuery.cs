using AutoMapper;
using Azure.Core;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;

namespace SaysanPwa.Application.Query.Factor;


public class GetAllSaleFactorsCountQuery : IRequest<List<GetSaleFactorDto>>
{
    public int FiscalYear { get; set; }
    public int UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public GetAllSaleFactorsCountQuery(int fiscalYear, int userId, string Date1, string Date2)
    {
        FiscalYear = fiscalYear;
        UserId = userId;
        From = Date1;
        To = Date2;
    }
}

public class GetAllSaleFactorsCountQueryHandler : IRequestHandler<GetAllSaleFactorsCountQuery, List<GetSaleFactorDto>>
{
    private readonly IFactorRepository _repository;
    private readonly IMapper _mapper;

    public GetAllSaleFactorsCountQueryHandler(IFactorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;

    }
    public async Task<List<GetSaleFactorDto>> Handle(GetAllSaleFactorsCountQuery request, CancellationToken cancellationToken)
    {

        var a = await _repository.GetAllSaleFactorsCount(request.FiscalYear, request.UserId, request.From, request.To ,cancellationToken);
        List<GetSaleFactorDto> result = _mapper.Map<List<GetSaleFactorDto>>(a);
        return result;
    }
}
