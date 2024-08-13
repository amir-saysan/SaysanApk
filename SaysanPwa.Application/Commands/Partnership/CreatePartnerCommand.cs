using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;
using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.Commands.Partnership;

public class CreatePartnerCommand : IRequest<SysResult<bool>>
{
    public string Type_TarafHesab { get; set; } = string.Empty;
    [Required(ErrorMessage = "نام را وارد کنید")]
    public string Name_TarafHesab { get; set; } = string.Empty;
    public int ID_tbl_Group_TF { get; set; } = 1;
    [Required(ErrorMessage = "کد ملی را وارد کنید")]
    public string? CodeMelli_TarafHesab { get; set; } = string.Empty;
    public string? CodeEgtesad_TarafHesab { get; set; } = "";
    public int ID_tbl_Job { get; set; }
    public string? BirthDay_TarafHesab { get; set; } = "";
    public string? Marrid_Date_TarafHesab { get; set; } = "";
    public int ID_tbl_Ostan_Asli { get; set; } = 1;
    public int ID_tbl_SharOstan_Asli { get; set; } = 1;
    public string? CodePosti_Asli { get; set; } = "";
    public string? Tell_Asli { get; set; } = "";
    public string? Address_Asli { get; set; } = "";
    public string? ChelPhone_TarafHesab { get; set; } = ""; 
    public string? Fax_TarafHesab { get; set; } = "";
    [EmailAddress]
    public string? Email_TarafHesab { get; set; } = "";
    public decimal Sagf_Eteb_Snd_TarafHesab { get; set; } = 0;
    public decimal Sagf_Eteb_Ngd_TarafHesab { get; set; } = 0;
    public bool Jelogiri_Had_Etebar_TarafHesab { get; set; }
    public List<AddBranchDto>? Branches { get; set; } = new();

    public string? Location_TarafHesab { get; set; } = "";


    public int UserId { get; set; }


    //#region toDelete
    //public int National_TarafHesab { get; set; }
    //public string State_TarafHesab { get; set; } = "فعال";
    //public bool State { get; set; } = true;
    //public bool Tamin_Konande_TarafHesab { get; set; }
    //public decimal Aval_Mnd_TaminKonande { get; set; }
    //public string Aval_Type_TaminKonande { get; set; } = "بی حساب";
    //public int Darayi_TaminKonande { get; set; }
    //public bool Kharidar_TarafHesab { get; set; }
    //public decimal Aval_Mnd_Kharidar { get; set; }
    //public string Aval_Type_Kharidar { get; set; } = "بی حساب";
    //public int Darayi_Kharidar { get; set; }
    //public string? Website_TarafHesab { get; set; }
    //public int? ID_tbl_Ostan { get; set; } = 1;
    //public int? ID_tbl_SharOstan { get; set; } = 1;
    //public string? CodePosti_Home_TarafHesab { get; set; }
    //public string? Tell_Home_TarafHesab { get; set; }
    //public string? Address_Home_TarafHesab { get; set; }
    //public int? ID_tbl_Ostan1 { get; set; }
    //public int? ID_tbl_SharOstan1 { get; set; }
    //public string? CodePosti_MahaleKar_TarafHesab { get; set; }
    //public string? Tell_MahaleKar_TarafHesab { get; set; }
    //public string? Address_MahaleKar_TarafHesab { get; set; }
    //public int? ID_tbl_Bank { get; set; }
    //public int? ID_tbl_BanchBank { get; set; }
    //public int? ID_tbl_TypeHesab { get; set; }
    //public string? HesabNumber_TarafHesab { get; set; }
    //public bool? Bzy_TarafHesab { get; set; }
    //public bool Karmand_TarafHesab { get; set; }
    //public decimal Aval_Mnd_Karmand { get; set; }
    //public string Aval_Type_Karmand { get; set; } = "بی حساب";
    //public int? Jensyat_Karmand_TarafHesab { get; set; }
    //public int? ID_tbl_Ostan_Tv { get; set; }
    //public int? ID_tbl_SharOstan_Tv { get; set; }
    //public int? ID_tbl_Ostan_Sd { get; set; }
    //public int? ID_tbl_SharOstan_Sd { get; set; }
    //public string? Shomare_Shenasname_Karmand_TarafHesab { get; set; }
    //public string? Serial_Shenasname_Karmand_TarafHesab { get; set; }
    //public string? Fathername_Karmand_TarafHesab { get; set; }
    //public int? MadrakTahsily_Karmand_TarafHesab { get; set; }
    //public int? Sarbazy_Karmand_TarafHesab { get; set; }
    //public int? Marrid_Karmand_TarafHesab { get; set; }
    //public int? Tedad_Farzand_Karmand_TarafHesab { get; set; }
    //public int? Tedad_Takafol_Karmand_TarafHesab { get; set; }
    //public string? Dt_Es { get; set; }
    //public string? Dt_Pay { get; set; }
    //public string? Number_Bime_Karmand_TarafHesab { get; set; }
    //public string? Number_BimeTakmily_Karmand_TarafHesab { get; set; }
    //public int? SabegeBime_Karmand_TarafHesab { get; set; }
    //public int Typ_Mal { get; set; }
    //public string Title_Job { get; set; } = null!;
    //public string Name_Group { get; set; } = null!;
    //public string? Name_Ostan { get; set; } = null!;
    //public string? Name_SharOstan { get; set; } = null!;
    //#endregion toDelete
}

public class CreatePartnerCommandHandler : IRequestHandler<CreatePartnerCommand, SysResult<bool>>
{
    private readonly IPartnerRepository _repository;
    private readonly IMapper _mapper;

    public CreatePartnerCommandHandler(IPartnerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SysResult<bool>> Handle(CreatePartnerCommand request, CancellationToken cancellationToken)
    {
        var test = _mapper.Map<List<Branch>>(request.Branches);
        var response = await _repository.AddAsync(request.UserId, new()
        {
            Code_TarafHesab = 0,
            Type_TarafHesab = request.Type_TarafHesab,
            Type_TarafHesab_Samane_Moadyan = 1,
            ID_tbl_Group_TF = request.ID_tbl_Group_TF == 0 ? 1 : request.ID_tbl_Group_TF,
            National_TarafHesab = 0,
            Name_TarafHesab = request.Name_TarafHesab,
            CodeMelli_TarafHesab = request.CodeMelli_TarafHesab,
            CodeEgtesad_TarafHesab = request.CodeEgtesad_TarafHesab ?? "",
            State_TarafHesab = "فعال",
            State = true,
            ID_tbl_Job = request.ID_tbl_Job,
            BirthDay_TarafHesab = request.Marrid_Date_TarafHesab ?? "",
            Marrid_Date_TarafHesab = request.BirthDay_TarafHesab ?? "",
            Tamin_Konande_TarafHesab = false,
            Aval_Mnd_TaminKonande = 0,
            Aval_Type_TaminKonande = "بی حساب",
            Darayi_TaminKonande = 0,
            Kharidar_TarafHesab = true,
            Aval_Mnd_Kharidar = 0,
            Aval_Type_Kharidar = "بی حساب",
            Darayi_Kharidar = 0,
            Jelogiri_Had_Etebar_TarafHesab = request.Jelogiri_Had_Etebar_TarafHesab,
            ChelPhone_TarafHesab = request.ChelPhone_TarafHesab ?? "",
            Fax_TarafHesab = request.Fax_TarafHesab ?? "",
            Email_TarafHesab = request.Email_TarafHesab ?? "",
            Website_TarafHesab = "",
            ID_tbl_Ostan = 1,
            ID_tbl_SharOstan = 1,
            CodePosti_Home_TarafHesab = "",
            Tell_Home_TarafHesab = "",
            Address_Home_TarafHesab = "",
            CodePosti_MahaleKar_TarafHesab = "",
            Tell_MahaleKar_TarafHesab = "",
            Address_MahaleKar_TarafHesab = "",
            ID_tbl_Ostan_Asli = request.ID_tbl_Ostan_Asli,
            ID_tbl_SharOstan_Asli = request.ID_tbl_SharOstan_Asli,
            CodePosti_Asli = request.CodePosti_Asli ?? "",
            Tell_Asli = request.Tell_Asli ?? "",
            Address_Asli = request.Address_Asli ?? "",
            ID_tbl_Bank = 1,
            ID_tbl_BanchBank = 1,
            ID_tbl_TypeHesab = 1,
            HesabNumber_TarafHesab = "",
            Sagf_Eteb_Snd_TarafHesab = request.Sagf_Eteb_Snd_TarafHesab,
            Sagf_Eteb_Ngd_TarafHesab = request.Sagf_Eteb_Ngd_TarafHesab,
            Bzy_TarafHesab = false,
            Karmand_TarafHesab = false,
            Aval_Mnd_Karmand = 0,
            Aval_Type_Karmand = "بی حساب",
            Jensyat_Karmand_TarafHesab = 0,
            ID_tbl_Ostan_Tv = 1,
            ID_tbl_SharOstan_Tv = 1,
            ID_tbl_Ostan_Sd = 1,
            ID_tbl_SharOstan_Sd = 1,
            Shomare_Shenasname_Karmand_TarafHesab = "",
            Serial_Shenasname_Karmand_TarafHesab = "",
            Fathername_Karmand_TarafHesab = "",
            MadrakTahsily_Karmand_TarafHesab = 0,
            Sarbazy_Karmand_TarafHesab = 0,
            Marrid_Karmand_TarafHesab = 0,
            Tedad_Farzand_Karmand_TarafHesab = 0,
            Tedad_Takafol_Karmand_TarafHesab = 0,
            Dt_Es = "/  /",
            Dt_Pay = "/  /",
            Number_Bime_Karmand_TarafHesab = "",
            Number_BimeTakmily_Karmand_TarafHesab = "",
            SabegeBime_Karmand_TarafHesab = 0,
            Typ_Mal = 0,
            ID_tbl_Ostan1 = 1,
            ID_tbl_SharOstan1 = 1,
            Location_TarafHesab = request.Location_TarafHesab ?? ""
        }, _mapper.Map<List<Branch>>(request.Branches) ,cancellationToken);
        return response;
    }
}