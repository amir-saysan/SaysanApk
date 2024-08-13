namespace SaysanPwa.Application.DTOs.Factor;

public record AddPreFactorDto
{
    public long ID_tbl_TarafHesab { get; set; } //
    public long ID_tbl_Partner_Branch { get; set; }
    public long ID_tbl_Bzy { get; set; } = 1;
    public decimal Tkh_N_PF { get; set; } // takhfif nagdi kol factor
    public decimal Tkh_A_PF { get; set; } // takfif nagdi + radif ha
    public decimal JM_PF { get; set; } // mablagh maliat
    public decimal JAv_PF { get; set; }
    public decimal J_PF { get; set; } // mablagh nahayi factor
    public decimal J_Tedad_Asl_PF { get; set; } // kol tedat item
    public decimal Pf_F { get; set; } = 1; // sood factor forosh
    public decimal Prsnt_Bzy { get; set; } = 0;
    public decimal Pdsh_Bzy { get; set; } = 0;
    public string Dt_PF { get; set; } // tarikh mosavi payini
    public string Dt_C_PF { get; set; }
    public string Tm_C_PF { get; set; } // zaman
    public int ID_tbl_SalMaly { get; set; } = 1;
    public int ID_tbl_Users { get; set; } = 1;
    public IEnumerable<AddFactorItemDto> Items { get; set; } = null!;

}
