using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.AggregateModels.FiscalYear;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using System.Collections.Generic;

namespace SaysanPwa.Application.Query.Factor;

public record GetCkeckSaleFactorMoadian : IRequest<string>
{
    public long ID_Tbl_FF { get; set; }
    public int ID_tbl_SalMaly { get; set; }
    public int typesorathesab { get; set; }
    public int subjectsorathesab { get; set; }
	//ID_FF, typesorathesab, subjectsorathesab

	public GetCkeckSaleFactorMoadian(long ID_FF, int subject_sorathesab, int type_sorathesab, int ID_SalMaly)
    {
		ID_Tbl_FF = ID_FF;
		typesorathesab = type_sorathesab;
        subjectsorathesab = subject_sorathesab;
		ID_tbl_SalMaly = ID_SalMaly;


	}
}

public class GetCkeckSaleFactorMoadianHandler : IRequestHandler<GetCkeckSaleFactorMoadian, string>
{
    private readonly IFactorRepository _factorRepository;
    private readonly IMapper _mapper;

    public GetCkeckSaleFactorMoadianHandler(IMapper mapper, IFactorRepository factorRepository)
    {
        _mapper = mapper;
        _factorRepository = factorRepository;
    }

    public async Task<string> Handle(GetCkeckSaleFactorMoadian request, CancellationToken cancellationToken)
    {

        var result = (await _factorRepository.GetSaleFactorsMoadianForSend(request.ID_Tbl_FF, request.typesorathesab, request.subjectsorathesab, request.ID_tbl_SalMaly));

		return result;
    }
}

