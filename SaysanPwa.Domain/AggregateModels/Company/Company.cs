using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.Company;

public class Company : Entity, IAggregateRoot
{
    public string Name_Comp { get; set; } = string.Empty;
    public string Address1_Comp { get; set; } = string.Empty;
}
