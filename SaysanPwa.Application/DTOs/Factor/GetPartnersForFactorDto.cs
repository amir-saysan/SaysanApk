namespace SaysanPwa.Application.DTOs.Factor;

public record GetPartnersForFactorDto
{
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Partner_Branch { get; set; }
    public string? Name_TarafHesab { get; set; }
    public string? Code_TarafHesab { get; set; }
    public string? Name_Branch { get; set; }
    public string? Brnach_Asli_Farei { get; set; }
}
