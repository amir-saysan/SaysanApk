namespace SaysanPwa.Domain.AggregateModels.AccountingDocument;

internal class DocumentMain
{
    public long ID_tbl_S1 { get; set; }
    public long Number_S1 { get; set; }
    public string Date_S1 { get; set; }
    public string Time_S1 { get; set; }
    public string Desc_S1 { get; set; }
    public string Type_S1 { get; set; }
    public string Type_ES1 { get; set; }
    public string Vazyat_S1 { get; set; }
    public decimal J_S1 { get; set; }
    public string Date_C_S1 { get; set; }
    public string Time_C_S1 { get; set; }
    public bool Comb_S1 { get; set; } = false;
    public int ID_tbl_SalMaly { get; set; }
    public int ID_tbl_Users { get; set; }
    public string Dt_Up { get; set; } = "";
    public string Tm_Up { get; set; } = "";
    public int ID_tbl_Users_Up { get; set; } = 0;
}
