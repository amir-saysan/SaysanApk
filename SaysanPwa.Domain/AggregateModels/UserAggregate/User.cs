using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.UserAggregate;

public class User : Entity, IAggregateRoot
{
    public int ID_tbl_Users { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool State_User { get; set; }
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Bzy { get; set; }
}