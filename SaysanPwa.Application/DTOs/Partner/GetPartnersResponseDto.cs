
namespace SaysanPwa.Application.DTOs.Partner;

public record GetPartnersResponseDto
{
    public long ID_tbl_TarafHesab { get; set; }
    public long Code_TarafHesab { get; set; }
    public string Type_TarafHesab { get; set; } = null!;
    public int? Type_TarafHesab_Samane_Moadyan { get; set; }
    public int ID_tbl_Group_TF { get; set; }
    public string Name_TarafHesab { get; set; } = null!;
    public string? CodeMelli_TarafHesab { get; set; }
    public string? CodeEgtesad_TarafHesab { get; set; }
    public string State_TarafHesab { get; set; } = null!;
    public bool State { get; set; }
    public int ID_tbl_Job { get; set; }
    public string? BirthDay_TarafHesab { get; set; }
    public string? Marrid_Date_TarafHesab { get; set; }
    public bool Tamin_Konande_TarafHesab { get; set; }
    public bool Kharidar_TarafHesab { get; set; }
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
    public int? Jensyat_Karmand_TarafHesab { get; set; }
}
