
namespace SaysanPwa.Application.DTOs.Partner;

public record AddBranchDto
{
    public long ID_tbl_TarafHesab { get; set; }
    public string Name_Branch { get; set; } = null!;
    public string Name_responsible { get; set; } = null!;
    public int? ID_tbl_Ostan { get; set; }
    public int? ID_tbl_SharOstan { get; set; }
    public string? Tell { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? CodePosti { get; set; }
    public string? Address { get; set; }
    public string? Location_Branch { get; set; }
}
