using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.Attributes;

namespace SaysanPwa.Domain.AggregateModels.FiscalYear;

public class FiscalYear : Entity, IAggregateRoot
{
    [Field("ID_tbl_SalMaly")]
    public int Id { get; set; }

    [Field("Title_SalMaly")]
    public string Title { get; set; } = string.Empty;

    [Field("BeginDate_SalMaly")]
    public string BeginDate { get; set; } = string.Empty;

    [Field("EndDate_SalMaly")]
    public string EndDate { get; set; } = string.Empty;

    [Field("ID_tbl_Users")]
    public int UserId { get; set; }
}
