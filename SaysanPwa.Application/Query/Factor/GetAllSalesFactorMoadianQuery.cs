﻿using AutoMapper;
using Azure.Core;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;

namespace SaysanPwa.Application.Query.Factor;


public class GetAllSalesFactorMoadianQuery : IRequest<List<TaxSaleDto>>
{
    public int FiscalYear { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public GetAllSalesFactorMoadianQuery(int fiscalYear, string Date1, string Date2)
    {
        FiscalYear = fiscalYear;
        From = Date1;
        To = Date2;
    }
}

public class GetAllSalesFactorMoadianQueryHandler : IRequestHandler<GetAllSalesFactorMoadianQuery, List<TaxSaleDto>>
{
    private readonly IFactorRepository _repository;
    private readonly IMapper _mapper;

    public GetAllSalesFactorMoadianQueryHandler(IFactorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;

    }
    public async Task<List<TaxSaleDto>> Handle(GetAllSalesFactorMoadianQuery request, CancellationToken cancellationToken)
    {
        var a = await _repository.GetAllSaleFactorsMoadian(request.FiscalYear, request.From, request.To ,cancellationToken);
        List<TaxSaleDto> result = _mapper.Map<List<TaxSaleDto>>(a);
        return result;
    }
}
