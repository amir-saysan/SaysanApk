using SaysanPwa.Application.DTOs.Factor;
using AutoMapper;
using MediatR;
using SaysanPwa.Domain.AggregateModels.Factor;
using System.Collections.Generic;

namespace SaysanPwa.Application.Query.Factor;

public class GetServiceSaleFactorPrintQuery : IRequest<List<GetServiceSaleFactorDto>>
{
    public long? FactorId { get; set; }
    public long? fiscalYear { get; set; }
    public int? UserId { get; set; }
    public int? Offset { get; set; }
    public int? ID_tbl_Bzy { get; set; }
    public string? Date1 { get; set; }
    public string? Date2 { get; set; }


    //int PishForoshID, int fiscalYear, int UserId, int Offset, int ID_tbl_Bzy, string Date1, string Date2

    public GetServiceSaleFactorPrintQuery(long? PFId, long? FiscalYear, int? userId, int? OFfset, int? IDBzy, string? D1, string? D2)
    {
        FactorId = PFId;
        fiscalYear = FiscalYear;
        UserId = userId;
        Offset = OFfset;
        ID_tbl_Bzy = IDBzy;
        Date1 = D1;
        Date2 = D2;

    }
}

public class GetServiceSaleFactorPrintQueryHanelr : IRequestHandler<GetServiceSaleFactorPrintQuery, List<GetServiceSaleFactorDto>>
{
    private readonly IFactorRepository _repo;
    private readonly IMapper _mapper;

    public GetServiceSaleFactorPrintQueryHanelr(IFactorRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<GetServiceSaleFactorDto>> Handle(GetServiceSaleFactorPrintQuery request, CancellationToken cancellationToken)
    {
        List<Domain.AggregateModels.Factor.SaleServiceFactor> Factor = await _repo.GetServiceSaleFactorPrint(request.FactorId, request.fiscalYear, request.UserId, request.Offset, request.ID_tbl_Bzy, request.Date1, request.Date2);
        List<GetServiceSaleFactorDto> result = _mapper.Map<List<GetServiceSaleFactorDto>>(Factor);
        return result;
    }
}
