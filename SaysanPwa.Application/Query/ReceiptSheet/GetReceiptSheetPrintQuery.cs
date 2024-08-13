using SaysanPwa.Application.DTOs.ReceiptSheet;
using AutoMapper;
using MediatR;
using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;
using System.Collections.Generic;

namespace SaysanPwa.Application.Query.ReceiptSheet;

public class GetReceiptSheetPrintQuery : IRequest<List<ReceiptSheetBaseDto>>
{
	public long? ID_tbl_DA { get; set; }
	public long? fiscalYear { get; set; }
	public int? UserId { get; set; }
	public int? Offset { get; set; }
	public string? Date1 { get; set; }
	public string? Date2 { get; set; }


	//int PishForoshID, int fiscalYear, int UserId, int Offset, int ID_tbl_Bzy, string Date1, string Date2

	public GetReceiptSheetPrintQuery(long? Id_tbl_DA, long? FiscalYear, int? userId, int? OFfset, int? IDBzy, string? D1, string? D2)
	{
		ID_tbl_DA = Id_tbl_DA;
		fiscalYear = FiscalYear;
		UserId = userId;
		Offset = OFfset;
		Date1 = D1;
		Date2 = D2;

	}
}

public class GetReceiptSheetPrintQueryHanelr : IRequestHandler<GetReceiptSheetPrintQuery, List<ReceiptSheetBaseDto>>
{
	private readonly IReceiptSheetRepository _repo;
	private readonly IMapper _mapper;

	public GetReceiptSheetPrintQueryHanelr(IReceiptSheetRepository repo, IMapper mapper)
	{
		_repo = repo;
		_mapper = mapper;
	}

	public async Task<List<ReceiptSheetBaseDto>> Handle(GetReceiptSheetPrintQuery request, CancellationToken cancellationToken)
	{
		List<Domain.AggregateModels.ReceiptSheetAggregate.ReceiptSheet> Factor = await _repo.GetReceiptSheetPrint(request.ID_tbl_DA, request.fiscalYear, request.UserId, request.Offset, request.Date1, request.Date2);
		List<ReceiptSheetBaseDto> result = _mapper.Map<List<ReceiptSheetBaseDto>>(Factor);
		return result;
	}
}
