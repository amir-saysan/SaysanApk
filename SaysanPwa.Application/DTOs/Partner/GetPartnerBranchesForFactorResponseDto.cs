namespace SaysanPwa.Application.DTOs.Partner;

public record GetPartnerBranchesForFactorResponseDto
{
    public long ID_tbl_Partner_Branch { get; set; }
    public string? Name_Branch { get; set; }
    public string? Brnach_Asli_Farei { get; set; }
}
