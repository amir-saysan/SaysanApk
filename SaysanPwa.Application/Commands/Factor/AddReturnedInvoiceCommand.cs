using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.SeedWorker;
using System.Globalization;
using System.Text.Json;

namespace SaysanPwa.Application.Commands.Factor;

public class AddReturnedInvoiceCommand: IRequest<SysResult<bool>>
{
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Partner_Branch { get; set; }
    public decimal Tkh_N_FBB { get; set; }
    public decimal Tkh_A_FBB { get; set; }
    public string Name_TarafHesab { get; set; }
    public long N_FBB { get; set; }
    public decimal JM_FBB { get; set; }
    public decimal JAv_FBB { get; set; }
    public decimal J_F_FBB { get; set; }
    public decimal BG_FBB { get; set; } = 0;
    public decimal J_Tedad_Asl_FBB { get; set; }
    public decimal J_Tedad_Fare_FBB { get; set; }
    public string Dc_FBB { get; set; } = "";
    public long ID_tbl_Bzy { get; set; }
    public decimal Prsnt_Bzy { get; set; } = 0;
    public string Dt_FBB { get; set; }
    public string Dt_C_FBB { get; set; } //Dt_FBB;
    public string Tm_C_FBB { get; set; }
    public long ID_tbl_S1 { get; set; } = 0;
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }

    public IEnumerable<AddFactorItemDto> Items { get; set; } = new List<AddFactorItemDto>();







    // ------------- For set fiscal year information to sale factor.

    public string FiscalYearBeginDate { get; set; } = string.Empty;
    public string FiscalYearEndDate { get; set; } = string.Empty;
    public string FiscalYearTitle { get; set; } = string.Empty;
}


public class AddReturnedInvoiceCommandHandler : IRequestHandler<AddReturnedInvoiceCommand, SysResult<bool>>
{
    private readonly IFactorRepository _repository;
    private readonly IMapper _mapper;

    public AddReturnedInvoiceCommandHandler(IFactorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    
    public async Task<SysResult<bool>> Handle(AddReturnedInvoiceCommand request, CancellationToken cancellationToken)
    {
        ReturnedInvoice returnedInvoice = _mapper.Map<ReturnedInvoice>(request);

        PersianCalendar pc = new PersianCalendar();
        DateTime now = DateTime.Now;
        returnedInvoice.Dt_FBB = $"{pc.GetYear(now)}/{pc.GetMonth(now).ToString("D2")}/{pc.GetDayOfMonth(now).ToString("D2")}";
        returnedInvoice.Dt_C_FBB = returnedInvoice.Dt_FBB;
        returnedInvoice.Tm_C_FBB = DateTime.Now.ToString("HH:mm:ss");
        foreach (AddFactorItemDto i in request.Items)
        {
            i.Fi_Bed_Haz = i.Fi_Sadere;
            i.Fi_Ba_Haz = i.Fi_Ba_Takh;
            i.Fi_Sadere = i.Fi_Ba_Takh;
            i.Mablagh_Sadere = i.Fi_Ba_Haz * i.Tedad;
            i.V_Asl = i.Tedad;
            i.Tedad_Sadere = i.Tedad;
            i.M_AV_Radf_K = i.MA_Radf_K;
        }
        if (string.IsNullOrEmpty(returnedInvoice.Dc_FBB))
        {
            returnedInvoice.Dc_FBB = "";
        }
        IEnumerable<FactorItem> items = _mapper.Map<IEnumerable<FactorItem>>(request.Items);



        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(returnedInvoice));
        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(items));

        var result = _repository.AddReturnedInvoice(returnedInvoice, items);
        return result.Result;
    }
}