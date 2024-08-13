

using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.DTOs.VisitPath;

public record EditPathDto
{
    public long ID_tbl_Bzy { get; set; }
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Partner_Branch { get; set; }
    [Required(ErrorMessage = "توضیحی وارد کنید")]
    public string? Description_Visited { get; set; }
}
