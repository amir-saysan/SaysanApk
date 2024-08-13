using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.SeedWorker;
using System.Globalization;

namespace SaysanPwa.Application.Commands.Factor;

public class AddFactorCommand : IRequest<SysResult<bool>>
{
    public long ID_tbl_TarafHesab { get; set; } //
    public long ID_tbl_Partner_Branch { get; set; }
    public long ID_tbl_Bzy { get; set; } = 0;
    public decimal Tkh_N_PF { get; set; } // kol mabkagh takfif -- mosavi payini
    public decimal Tkh_A_PF { get; set; }
    public decimal JM_PF { get; set; } // mablagh JAv_PF
    public decimal JAv_PF { get; set; }
    public decimal J_PF { get; set; } // mablagh nahayi factor
    public decimal J_Tedad_Asl_PF { get; set; } // kol tedat item
    public decimal Pf_F { get; set; } = 1; // sood factor forosh
    public decimal Prsnt_Bzy { get; set; } = 0;
    public decimal Pdsh_Bzy { get; set; } = 0;
    public string Dt_PF { get; set; } // tarikh mosavi payini
    public string Dt_C_PF { get; set; }
    public string Tm_C_PF { get; set; } // zaman
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }
    public IEnumerable<AddFactorItemDto> Items { get; set; } = null!;
}

public class AddFactorCommandHandler : IRequestHandler<AddFactorCommand, SysResult<bool>>
{
    private readonly IFactorRepository _factorRepository;
    private readonly IMapper _mapper;


    public AddFactorCommandHandler(IFactorRepository factorRepository, IMapper mapper)
    {
        _factorRepository = factorRepository;
        _mapper = mapper;
    }
    public Task<SysResult<bool>> Handle(AddFactorCommand request, CancellationToken cancellationToken)
    {
        PreFactor preFactor = _mapper.Map<PreFactor>(request);

        PersianCalendar pc = new PersianCalendar();
        DateTime now = DateTime.Now;
        preFactor.Dt_PF = $"{pc.GetYear(now)}/{pc.GetMonth(now).ToString("D2")}/{pc.GetDayOfMonth(now).ToString("D2")}";
        preFactor.Dt_C_PF = preFactor.Dt_PF;
        preFactor.Tm_C_PF = DateTime.Now.ToString("HH:mm:ss");
        foreach(AddFactorItemDto i in request.Items)
        {
            i.Fi_Bed_Haz = i.Fi_Sadere;
            i.Fi_Ba_Haz = i.Fi_Ba_Takh;
            i.Fi_Sadere = i.Fi_Ba_Takh;
            i.Mablagh_Sadere = i.Fi_Ba_Haz * i.Tedad;
            i.V_Asl = i.Tedad;
            i.Tedad_Sadere = i.Tedad;
            i.M_AV_Radf_K = i.MA_Radf_K;
        }
        IEnumerable<FactorItem> items = _mapper.Map<IEnumerable<FactorItem>>(request.Items);


        var result = _factorRepository.AddPreFactor(preFactor, items);
        return result;
    }
}
