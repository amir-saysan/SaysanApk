namespace SaysanPwa.Application.DTOs.Factor;

public record AddFactorDto
{
    public long ID_tbl_Kala { get; set; }
    public int ID_tbl_Anbar { get; set; }
    public int ID_tblVahede_Kala { get; set; }
    public decimal Tedad { get; set; }
    public decimal Fi_Bed_Haz { get; set; }
    public decimal Fi_Ba_Takh { get; set; }
    public decimal Fi_Ba_Haz { get; set; }
    public decimal M_T_Radf_K { get; set; }
    public decimal D_T_Radf_K { get; set; }
    public decimal MA_Radf_K { get; set; }
    public decimal AV_Radf_K { get; set; }
    public decimal M_AV_Radf_K { get; set; }
    public decimal V_Asl { get; set; }
    public decimal V_Fare { get; set; }
    public decimal M_KHLS { get; set; }
    public decimal Tedad_Sadere { get; set; }
    public decimal Fi_Sadere { get; set; }
    public decimal Mablagh_Sadere { get; set; }
    public decimal Mnd_Megdar { get; set; }
    public decimal Mnd_Fi { get; set; }
    public decimal Mnd_Mablagh { get; set; }
    public bool jj { get; set; } = false;
    public long ID_tbl_F { get; set; }
    public string Type_tbl_F { get; set; }
    public int ID_tbl_SalMaly { get; set; }
    public IEnumerable<AddFactorItemDto> Items { get; set; } = null!;
}

