using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

namespace SaysanPwa.Application.Query.ReceiptSheet;


public class GetAllReceiptSheetsQuery : IRequest<List<GetReceiptSheetDto>>
{
	public int FiscalYear { get; set; }
	public int UserId { get; set; }
	public string From { get; set; }
	public string To { get; set; }

	public GetAllReceiptSheetsQuery(int fiscalYear, int userId, string Date1, string Date2)
	{
		FiscalYear = fiscalYear;
		UserId = userId;
		From = Date1;
		To = Date2;
	}
}

public class GetAllReceiptSheetsQueryHandler : IRequestHandler<GetAllReceiptSheetsQuery, List<GetReceiptSheetDto>>
{
	private readonly IReceiptSheetRepository _repository;
	private readonly IMapper _mapper;

	public GetAllReceiptSheetsQueryHandler(IReceiptSheetRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;

	}
	public async Task<List<GetReceiptSheetDto>> Handle(GetAllReceiptSheetsQuery request, CancellationToken cancellationToken)
	{
		var a = await _repository.GetAllReceiptSheetsCount(request.FiscalYear, request.UserId, request.From, request.To, cancellationToken);
		List<GetReceiptSheetDto> result = _mapper.Map<List<GetReceiptSheetDto>>(a);
		return result;
	}
}
