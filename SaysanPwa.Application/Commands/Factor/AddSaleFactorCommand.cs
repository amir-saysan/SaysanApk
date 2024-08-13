using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.SeedWorker;
using System.Globalization;
using System.Text.Json;

namespace SaysanPwa.Application.Commands.Factor;

public class AddSaleFactorCommand : IRequest<SysResult<bool>>
{
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Partner_Branch { get; set; }
    public decimal Tkh_N_F { get; set; }
    public decimal Tkh_A_F { get; set; }
    public decimal JM_F { get; set; }
    public decimal JAv_F { get; set; }
    public decimal J_F { get; set; }
    public decimal BG_F { get; set; }
    public decimal J_Tedad_Asl_F { get; set; }
    public decimal J_Tedad_Fare_F { get; set; }
    public string Dc_F { get; set; } = "";
    public long ID_tbl_Bzy { get; set; }
    public decimal Prsnt_Bzy { get; set; } = 0;
    public decimal Pdsh_Bzy { get; set; } = 0;
    public string Dt_F { get; set; }
    public string Dt_C_F { get; set; } //Dt_F;
    public string Tm_C_F { get; set; }
    public long ID_tbl_S1 { get; set; } = 0;
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }

    public IEnumerable<AddFactorItemDto> Items { get; set; } = new List<AddFactorItemDto>();







    // ------------- For set fiscal year information to sale factor.

    public string FiscalYearBeginDate { get; set; } = string.Empty;
    public string FiscalYearEndDate { get; set; } = string.Empty;
    public string FiscalYearTitle { get; set; } = string.Empty;
}


public class AddSaleFactorCommandHandler : IRequestHandler<AddSaleFactorCommand, SysResult<bool>>
{
    private readonly IFactorRepository _repository;
    private readonly IMapper _mapper;

    public AddSaleFactorCommandHandler(IFactorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    
    public async Task<SysResult<bool>> Handle(AddSaleFactorCommand request, CancellationToken cancellationToken)
    {
        SaleFactor saleFactor = _mapper.Map<SaleFactor>(request);

        PersianCalendar pc = new PersianCalendar();
        DateTime now = DateTime.Now;
        saleFactor.Dt_F = $"{pc.GetYear(now)}/{pc.GetMonth(now).ToString("D2")}/{pc.GetDayOfMonth(now).ToString("D2")}";
        saleFactor.Dt_C_F = saleFactor.Dt_F;
        saleFactor.Tm_C_F = DateTime.Now.ToString("HH:mm:ss");
        saleFactor.BG_F = saleFactor.J_F;
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
        if (string.IsNullOrEmpty(saleFactor.Dc_F))
        {
            saleFactor.Dc_F = "";
        }
        IEnumerable<FactorItem> items = _mapper.Map<IEnumerable<FactorItem>>(request.Items);



        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(saleFactor));
        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(items));

        var result = _repository.AddSaleFactor(saleFactor, items);
        return result.Result;
    }
}