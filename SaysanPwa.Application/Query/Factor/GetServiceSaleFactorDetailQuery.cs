using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using System.Collections.Generic;

namespace SaysanPwa.Application.Query.Factor;

public record GetServiceSaleFactorDetailQuery(long id, int ID_tbl_SalMaly) : IRequest<List<ServiceSaleFactorDetailDto>>;

public class GetServiceSaleFactorDetailQueryHandler : IRequestHandler<GetServiceSaleFactorDetailQuery, List<ServiceSaleFactorDetailDto>>
{
	private readonly IFactorRepository _FactorRepository;
	private readonly IMapper _mapper;

	public GetServiceSaleFactorDetailQueryHandler(IMapper mapper, IFactorRepository ServiceSaleFactorRepository)
	{
		_mapper = mapper;
		_FactorRepository = ServiceSaleFactorRepository;
	}

	public async Task<List<ServiceSaleFactorDetailDto>> Handle(GetServiceSaleFactorDetailQuery request, CancellationToken cancellationToken)
	{
		List<SaleServiceFactorDetailViewModel> ServiceSaleFactor = (await _FactorRepository.GetServiceSaleFactorDetailAsync(request.id, request.ID_tbl_SalMaly));
		List<ServiceSaleFactorDetailDto> result = _mapper.Map<List<ServiceSaleFactorDetailDto>>(ServiceSaleFactor);

		return result;
	}
}
