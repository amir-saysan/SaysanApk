
using AutoMapper.Configuration.Annotations;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;

namespace SaysanPwa.Application.DTOs.Partner;

public class PartnerDetailDto
{
    public long ID_tbl_TarafHesab { get; set; }
    public long Code_TarafHesab { get; set; }
    public string Type_TarafHesab { get; set; } = null!;
    public int? Type_TarafHesab_Samane_Moadyan { get; set; }
    public int ID_tbl_Group_TF { get; set; }
    public int National_TarafHesab { get; set; }
    public string Name_TarafHesab { get; set; } = null!;
    public string? CodeMelli_TarafHesab { get; set; }
    public string? CodeEgtesad_TarafHesab { get; set; }
    public string State_TarafHesab { get; set; } = null!;
    public bool State { get; set; }
    public int ID_tbl_Job { get; set; }
    public string? BirthDay_TarafHesab { get; set; }
    public string? Marrid_Date_TarafHesab { get; set; }
    public bool Tamin_Konande_TarafHesab { get; set; }
    public decimal Aval_Mnd_TaminKonande { get; set; }
    public string Aval_Type_TaminKonande { get; set; } = null!;
    public int Darayi_TaminKonande { get; set; }
    public bool Kharidar_TarafHesab { get; set; }
    public decimal Aval_Mnd_Kharidar { get; set; }
    public string Aval_Type_Kharidar { get; set; } = null!;
    public int Darayi_Kharidar { get; set; }
    public bool? Jelogiri_Had_Etebar_TarafHesab { get; set; }
    public string? ChelPhone_TarafHesab { get; set; }
    public string? Fax_TarafHesab { get; set; }
    public string? Email_TarafHesab { get; set; }
    public string? Website_TarafHesab { get; set; }
    public int ID_tbl_Ostan { get; set; }
    public int ID_tbl_SharOstan { get; set; }
    public string? CodePosti_Home_TarafHesab { get; set; }
    public string? Tell_Home_TarafHesab { get; set; }
    public string? Address_Home_TarafHesab { get; set; }
    public int? ID_tbl_Ostan1 { get; set; }
    public int? ID_tbl_SharOstan1 { get; set; }
    public string? CodePosti_MahaleKar_TarafHesab { get; set; }
    public string? Tell_MahaleKar_TarafHesab { get; set; }
    public string? Address_MahaleKar_TarafHesab { get; set; }
    public int? ID_tbl_Ostan_Asli { get; set; }
    public int? ID_tbl_SharOstan_Asli { get; set; }
    public string? CodePosti_Asli { get; set; }
    public string? Tell_Asli { get; set; }
    public string? Address_Asli { get; set; }
    public int? ID_tbl_Bank { get; set; }
    public int? ID_tbl_BanchBank { get; set; }
    public int? ID_tbl_TypeHesab { get; set; }
    public string? HesabNumber_TarafHesab { get; set; }
    public decimal? Sagf_Eteb_Snd_TarafHesab { get; set; }
    public decimal? Sagf_Eteb_Ngd_TarafHesab { get; set; }
    public bool? Bzy_TarafHesab { get; set; }
    public bool Karmand_TarafHesab { get; set; }
    public decimal Aval_Mnd_Karmand { get; set; }
    public string Aval_Type_Karmand { get; set; } = null!;
    public int? Jensyat_Karmand_TarafHesab { get; set; }
    public int? ID_tbl_Ostan_Tv { get; set; }
    public int? ID_tbl_SharOstan_Tv { get; set; }
    public int? ID_tbl_Ostan_Sd { get; set; }
    public int? ID_tbl_SharOstan_Sd { get; set; }
    public string? Shomare_Shenasname_Karmand_TarafHesab { get; set; }
    public string? Serial_Shenasname_Karmand_TarafHesab { get; set; }
    public string? Fathername_Karmand_TarafHesab { get; set; }
    public int? MadrakTahsily_Karmand_TarafHesab { get; set; }
    public int? Sarbazy_Karmand_TarafHesab { get; set; }
    public int? Marrid_Karmand_TarafHesab { get; set; }
    public int? Tedad_Farzand_Karmand_TarafHesab { get; set; }
    public int? Tedad_Takafol_Karmand_TarafHesab { get; set; }
    public string? Dt_Es { get; set; }
    public string? Dt_Pay { get; set; }
    public string? Number_Bime_Karmand_TarafHesab { get; set; }
    public string? Number_BimeTakmily_Karmand_TarafHesab { get; set; }
    public int? SabegeBime_Karmand_TarafHesab { get; set; }
    public int Typ_Mal { get; set; }

    public string Title_Job { get; set; } = null!;
    public string Name_Group { get; set; } = null!;
    public string? Name_Ostan { get; set; } = null!;
    public string? Name_SharOstan { get; set; } = null!;

    public string? Location_TarafHesab { get; set; }

    public List<Branch>? Branches { get; set; } = new();
}
