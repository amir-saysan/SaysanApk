using AutoMapper;
using Azure.Core;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;

namespace SaysanPwa.Application.Query.Factor;


public class GetAllServiceFactorsCountQuery : IRequest<List<GetServiceSaleFactorDto>>
{
    public int FiscalYear { get; set; }
    public int UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public GetAllServiceFactorsCountQuery(int fiscalYear, int userId, string Date1, string Date2)
    {
        FiscalYear = fiscalYear;
        UserId = userId;
        From = Date1;
        To = Date2;
    }
}

public class GetAllServiceFactorsCountQueryHandler : IRequestHandler<GetAllServiceFactorsCountQuery, List<GetServiceSaleFactorDto>>
{
    private readonly IFactorRepository _repository;
    private readonly IMapper _mapper;

    public GetAllServiceFactorsCountQueryHandler(IFactorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<List<GetServiceSaleFactorDto>> Handle(GetAllServiceFactorsCountQuery request, CancellationToken cancellationToken)
    {

        var a = await _repository.GetAllServiceSaleFactorsCount(request.FiscalYear, request.UserId, request.From, request.To, cancellationToken);
        List<GetServiceSaleFactorDto> result = _mapper.Map<List<GetServiceSaleFactorDto>>(a);
        return result;
    }
}
