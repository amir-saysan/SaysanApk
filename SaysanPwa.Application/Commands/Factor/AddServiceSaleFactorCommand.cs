using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.SeedWorker;
using System.Globalization;

namespace SaysanPwa.Application.Commands.Factor;

public class AddServiceSaleFactorCommand : IRequest<SysResult<bool>>
{
    public string Type_tbl_F { get; set; } = "tbl_FF_KHed";
    public long N_FF_KHed { get; set; }
    public long ID_tbl_TarafHesab_Moj { get; set; } = 0;
    public decimal Pors_Mbg { get; set; } = 0;
    public bool rdo1 { get; set; } = false;
    public bool rdo2 { get; set; } = true;
    public decimal Tkh_N_FF_KHed { get; set; }
    public decimal Tkh_K_FF_KHed { get; set; } = 0;
    public decimal Tkh_A_FF_KHed { get; set; }
    public decimal JM_FF_KHed { get; set; }
    public decimal JAv_FF_KHed { get; set; }
    public decimal J_Hz_FF_KHed { get; set; } = 0;
    public decimal J_FF_KHed { get; set; }
    public decimal BG_FF_KHed { get; set; }
    public long J_Tedad_Asli_FF_KHed { get; set; }
    public long J_Tedad_Farei_FF_KHed { get; set; } = 0;
    public string Dc_FF_KHed { get; set; }
    public string Dt_FF_KHed { get; set; }
    public string Dt_C_FF_KHed { get; set; }
    public string Tm_C_FF_KHed { get; set; }
    public bool Conred_FF_KHed { get; set; } = false;
    public long ID_tbl_DA { get; set; } = 0;
    public long ID_tbl_S1 { get; set; }
    public string IS_PR { get; set; } = "چاپ نشده";
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }
    public string Dt_Up { get; set; } = "";
    public string Tm_Up { get; set; } = "";
    public int ID_tbl_Users_Up { get; set; } = 0;

    // ------------- For init fiscal year information to sale factor.

    public string FiscalYearBeginDate { get; set; } = string.Empty;
    public string FiscalYearEndDate { get; set; } = string.Empty;
    public string FiscalYearTitle { get; set; } = string.Empty;

    public long ID_tbl_TarafHesab { get; set; }

    public IEnumerable<AddSaleServiceFactorItemDto> Items { get; set; } = new List<AddSaleServiceFactorItemDto>();
}


public class AddServiceSaleFactorCommandHandler : IRequestHandler<AddServiceSaleFactorCommand, SysResult<bool>>
{
    private readonly IFactorRepository _repository;
    private readonly IMapper _mapper;

    public AddServiceSaleFactorCommandHandler(IFactorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<SysResult<bool>> Handle(AddServiceSaleFactorCommand request, CancellationToken cancellationToken)
    {
        SaleServiceFactor ServiceFactor = _mapper.Map<SaleServiceFactor>(request);

        PersianCalendar pc = new PersianCalendar();
        DateTime now = DateTime.Now;
        ServiceFactor.Dt_FF_KHed = $"{pc.GetYear(now)}/{pc.GetMonth(now).ToString("D2")}/{pc.GetDayOfMonth(now).ToString("D2")}";
        ServiceFactor.Dt_C_FF_KHed = $"{pc.GetYear(now)}/{pc.GetMonth(now).ToString("D2")}/{pc.GetDayOfMonth(now).ToString("D2")}";
        ServiceFactor.Tm_C_FF_KHed = DateTime.Now.ToString("HH:mm:ss");
        ServiceFactor.BG_FF_KHed = ServiceFactor.J_FF_KHed;
        foreach (AddSaleServiceFactorItemDto i in request.Items)
        {
            Console.WriteLine("i.Fi_Bed_Haz:" + i.Fi_Bed_Haz);
            Console.WriteLine("i.Fi_Ba_Takh:" + i.Fi_Ba_Takh);
            Console.WriteLine("i.Tedad:" + i.Tedad);
            Console.WriteLine("i.MA_Radf_K:" + i.MA_Radf_K);
            Console.WriteLine("i.Mbg_MA_Radf_K:" + i.Mbg_MA_Radf_K);
            Console.WriteLine("i.AV_Radf_K:" + i.AV_Radf_K);
            Console.WriteLine("i.Mbg_AV_Radf_K:" + i.Mbg_AV_Radf_K);
            i.Fi_Bed_Haz = i.Fi_Bed_Haz;
            i.Fi_Ba_Takh = i.Fi_Ba_Takh;
            i.Fi_Ba_Haz = i.Fi_Ba_Takh;
            i.V_Asl = i.Tedad;
            i.MA_Radf_K = i.MA_Radf_K;
            i.Mbg_MA_Radf_K = i.Mbg_MA_Radf_K;
            i.AV_Radf_K = i.AV_Radf_K;
            i.Mbg_AV_Radf_K = i.Mbg_AV_Radf_K;
        }
        IEnumerable<ServiceFactorItem> items = _mapper.Map<IEnumerable<ServiceFactorItem>>(request.Items);


        var result = _repository.AddSaleServiceFactor(ServiceFactor, items);
        return result;
    }
}