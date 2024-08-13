using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.AggregateModels.FiscalYear;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using System.Collections.Generic;

namespace SaysanPwa.Application.Query.Factor;

public record GetSaleFactorQuery : IRequest<List<GetSaleFactorDto>>
{
    public int Offset { get; set; }
    public int FiscalYear { get; set; }
    public int UserId { get; set; }
    public string Date1 { get; set; }
    public string Date2 { get; set; }

    public GetSaleFactorQuery(int fiscalYear, int userId, string Date1, string Date2, int offset = 0)
    {
        Offset = offset;
        FiscalYear = fiscalYear;
        UserId = userId;
        Date1 = Date1;
        Date2 = Date2;
    }
}

public class GetSaleFactorQueryHandler : IRequestHandler<GetSaleFactorQuery, IEnumerable<GetSaleFactorDto>>
{
    private readonly IFactorRepository _factorRepository;
    private readonly IMapper _mapper;

    public GetSaleFactorQueryHandler(IMapper mapper, IFactorRepository factorRepository)
    {
        _mapper = mapper;
        _factorRepository = factorRepository;
    }

    public async Task<IEnumerable<GetSaleFactorDto>> Handle(GetSaleFactorQuery request, CancellationToken cancellationToken)
    {

        IEnumerable<SaleFactor> factors = (await _factorRepository.GetSaleFactors(request.FiscalYear,request.UserId,request.Date1, request.Date2)).Result;
        IEnumerable<GetSaleFactorDto> result = _mapper.Map<IEnumerable<GetSaleFactorDto>>(factors);
        //foreach (var product in result)
        //{
        //    if (product.Pic_Kala is not null)
        //    {
        //        product.base64StringPicture = Convert.ToBase64String(product.Pic_Kala);
        //    }
        //}
        return result;
    }
}

