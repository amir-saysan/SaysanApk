/*using AutoMapper;
using MediatR;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Commands.Partnership;

public class AddNewPartnerCommand : IRequest<SysResult<bool>>
{
    public string Name { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public int GroupId { get; set; }
    public string? EconomicCode { get; set; }
    public int JobId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public int RegionId { get; set; }
    public int CityId { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public string LandlinePhone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public AddNewPartnerCommand(string name, string nationalId, int groupId, string economicCode, int jobId,
        string phoneNumber, int regionId, int cityId, string postalCode, string landLinePhone, string address) =>
        (Name, NationalId, GroupId, EconomicCode, JobId, PhoneNumber, RegionId, CityId, PostalCode, LandlinePhone, Address) =
        (name, nationalId, groupId, economicCode, jobId, phoneNumber, regionId, cityId, postalCode, landLinePhone, address);
}

public class AddNewPartnerCommandHandler : IRequestHandler<AddNewPartnerCommand, SysResult<bool>>
{
    private readonly IPartnerRepository _repository;

    public AddNewPartnerCommandHandler(IPartnerRepository repository) => _repository = repository;

    public async Task<SysResult<bool>> Handle(AddNewPartnerCommand request, CancellationToken cancellationToken) => await _repository.AddAsync(, new()
    {
        Code_TarafHesab = 0,
        Type_TarafHesab = "شخصی حقیقی",
        Type_TarafHesab_Samane_Moadyan = 1,
        ID_tbl_Group_TF = request.GroupId,
        National_TarafHesab = 0,
        Name_TarafHesab = request.Name,
        CodeMelli_TarafHesab = request.NationalId ?? null,
        CodeEgtesad_TarafHesab = request.EconomicCode ?? null,
        State_TarafHesab = "فعال",
        State = true,
        ID_tbl_Job = request.JobId,
        BirthDay_TarafHesab = "/  /",
        Marrid_Date_TarafHesab = "/  /",
        Tamin_Konande_TarafHesab = false,
        Aval_Mnd_TaminKonande = 0,
        Aval_Type_TaminKonande = "بی حساب",
        Darayi_TaminKonande = 0,
        Kharidar_TarafHesab = true,
        Aval_Mnd_Kharidar = 0,
        Aval_Type_Kharidar = "بی حساب",
        Darayi_Kharidar = 0,
        Jelogiri_Had_Etebar_TarafHesab = false,
        ChelPhone_TarafHesab = request.PhoneNumber,
        Fax_TarafHesab = "",
        Email_TarafHesab = "",
        Website_TarafHesab = "",
        ID_tbl_Ostan = request.RegionId,
        ID_tbl_SharOstan = request.CityId,
        CodePosti_Home_TarafHesab = "",
        Tell_Home_TarafHesab = "",
        Address_Home_TarafHesab = "",
        CodePosti_MahaleKar_TarafHesab = "",
        Tell_MahaleKar_TarafHesab = "",
        Address_MahaleKar_TarafHesab = "",
        ID_tbl_Ostan_Asli = request.RegionId,
        ID_tbl_SharOstan_Asli = request.CityId,
        CodePosti_Asli = request.PostalCode,
        Tell_Asli = request.LandlinePhone,
        Address_Asli = request.Address,
        ID_tbl_Bank = 1,
        ID_tbl_BanchBank = 1,
        ID_tbl_TypeHesab = 1,
        HesabNumber_TarafHesab = "",
        Sagf_Eteb_Snd_TarafHesab = 0,
        Sagf_Eteb_Ngd_TarafHesab = 0,
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
        ID_tbl_SharOstan1 = 1
    });
}*/