using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.DTOs.Partner;

public record EditPartnerRequestDto
{
    public long ID_tbl_Partner_Branch { get; set; }
    public long ID_tbl_TarafHesab { get; set; }
    public string Type_TarafHesab { get; set; } = null!;
    public int ID_tbl_Group_TF { get; set; }
    [Required(ErrorMessage = "نام طرفحساب نمیتواند خالی باشد.")]
    [MaxLength(150, ErrorMessage = "نام طرفحساب نمیتواند بیشتر از 150 حرف باشد")]
    [MinLength(2, ErrorMessage = "نام طرفحساب نمیتواند کمتر از 2 حرف باشد")]
    public string Name_TarafHesab { get; set; } = null!;
    public long Code_TarafHesab { get; set; }
    public string? CodeMelli_TarafHesab { get; set; } = string.Empty;
    public string? CodeEgtesad_TarafHesab { get; set; } = string.Empty;
    public int ID_tbl_Job { get; set; }
    public string? BirthDay_TarafHesab { get; set; } = string.Empty;
    public string? Marrid_Date_TarafHesab { get; set; } = string.Empty;
    public bool Jelogiri_Had_Etebar_TarafHesab { get; set; }
    public string? ChelPhone_TarafHesab { get; set; } = string.Empty;
    public string? Fax_TarafHesab { get; set; } = string.Empty;
    public string? Email_TarafHesab { get; set; } = string.Empty;
    public int ID_tbl_Ostan_Asli { get; set; }
    public int ID_tbl_SharOstan_Asli { get; set; }
    public string? CodePosti_Asli { get; set; } = string.Empty;
    public string? Tell_Asli { get; set; } = string.Empty;
    public string? Address_Asli { get; set; } = string.Empty;
    public int ID_tbl_Bank { get; set; }
    public int ID_tbl_BanchBank { get; set; }
    public int ID_tbl_TypeHesab { get; set; }
    public string? HesabNumber_TarafHesab { get; set; } = string.Empty;
    public decimal Sagf_Eteb_Snd_TarafHesab { get; set; } = 0;
    public decimal Sagf_Eteb_Ngd_TarafHesab { get; set; } = 0;
    public string? Title_Job { get; set; }
    public string? Name_Group { get; set; }
    public string? Name_Ostan { get; set; }
    public string? Name_SharOstan { get; set; }

    public string? Location_TarafHesab { get; set; } = string.Empty;


    public List<EditBranchDto>? Branches { get; set; } = new();
}
