using MediatR;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Commands.ReceiptSheet;

public class AddReceiptSheetCommand : IRequest<SysResult<bool>>
{
    public ReceiptSheetBaseDto ReceiptSheetBaseDto { get; set; } = new();
}

public class AddReceiptSheetCommandHandler : IRequestHandler<AddReceiptSheetCommand, SysResult<bool>>
{
    private readonly IReceiptSheetRepository _repository;

    public AddReceiptSheetCommandHandler(IReceiptSheetRepository repository)
    {
        _repository = repository;
    }

    public async Task<SysResult<bool>> Handle(AddReceiptSheetCommand request, CancellationToken cancellationToken)
    {

        return await _repository.AddReceiptSheet(new()
        {
            ID_tbl_TarafHesab = request.ReceiptSheetBaseDto.PartnerId,
            Typ_DA = request.ReceiptSheetBaseDto.ReceiptionType,
            Dt_DA = request.ReceiptSheetBaseDto.Date,
            Dc_DA = request.ReceiptSheetBaseDto.Description,
            ID_tbl_SalMaly = int.Parse(request.ReceiptSheetBaseDto.FiscalYear),
            ID_tbl_Users = request.ReceiptSheetBaseDto.CurrentUser,
            ID_tbl_Sandog = request.ReceiptSheetBaseDto.CashDesks.FirstOrDefault()?.CashDeskId,
            M_to_S = request.ReceiptSheetBaseDto.CashDesks.FirstOrDefault()?.Price,
            DC_To_S = request.ReceiptSheetBaseDto.CashDesks.FirstOrDefault()?.Description
        },
        request.ReceiptSheetBaseDto.Checks.Select(s => new tbl_Daryaft_Chek()
        { 
            Desc_Chek = s.Description,
            Shomare_Chek = s.CheckNumber,
            Mablagh_Chek = s.CheckAmount,
            PN = s.BehindTheCheckMark,
            Serial_Chek = s.Series,
            Dt_D_Chek = s.ReceiveDate,
            Dt_S_Chek = s.DuDate,
        }).ToList(),
        request.ReceiptSheetBaseDto.Remittances.Select(s => new tbl_DA_H()
        {
            N_H = s.RemittanceNumber,
            Mablag_DA_H = s.Price,
            DC_DA_H = s.Description,
            ID_tbl_Hesab = s.BankAccountId
        }).ToList(),
        request.ReceiptSheetBaseDto.CardReader.Select(s => new tbl_DA_K()
        {
            ID_tbl_Hesab = s.BankAccountId,
            Mablag_DA_K = s.Price,
            Peygiry_Nu = s.IssueTracking,
            Traction_Nu = s.TransactionSeries,
            DC_DA_K = s.Description
        }).ToList());

    }
}