using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.MarketerAggregate;

public class Marketer : Entity, IAggregateRoot
{
    public long ID_tbl_Bzy { get; set;}
    /// <summary>
    /// درصد فروش بازریاب از سقف 1
    /// </summary>
    public bool chk_SGF1 { get; set; }

    /// <summary>
    /// از مبلغ
    /// </summary>
    public decimal A1_MB_S_Bzy { get; set; }

    /// <summary>
    /// تا مبلغ
    /// </summary>
    public decimal T1_MB_S_Bzy { get; set; }

    /// <summary>
    /// درصد سقف یک
    /// </summary>
    public decimal D_F1_Bzy { get; set; }

    /// <summary>
    /// درصد فروش بازریاب از سقف 2
    /// </summary>
    public bool chk_SGF2 { get; set; }

    /// <summary>
    /// از مبلغ 2
    /// </summary>
    public decimal A2_MB_S_Bzy { get; set; }

    /// <summary>
    /// تا مبلغ 2
    /// </summary>
    public decimal T2_MB_S_Bzy { get; set; }

    /// <summary>
    /// درصد سقف 2
    /// </summary>
    public decimal D_F2_Bzy { get; set; }

    /// <summary>
    /// تعیین مبلغ باداش
    /// </summary>
    public bool chk_PAD { get; set; }

    /// <summary>
    /// مبلغ پاداش بازاریاب
    /// </summary>
    public decimal Mblgh_PAD_Bzy { get; set; }

    /// <summary>
    /// حقوق پایه
    /// </summary>
    public decimal Hog_P_Bzy { get; set; }

    public int ID_tbl_Ostan { get; set; }

    public int ID_tbl_SharOstan { get; set; }

    public int ID_tbl_Mant { get; set; }
    public int ID_tbl_MSR { get; set; }
    public bool State { get; set; }
    public bool State_Pors { get; set; }
}
