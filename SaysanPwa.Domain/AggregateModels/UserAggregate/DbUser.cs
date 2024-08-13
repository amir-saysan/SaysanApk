namespace SaysanPwa.Domain.AggregateModels.UserAggregate;

public class DbUser
{
    public int ID_tbl_Users { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool State_User { get; set; }
    public long ID_tbl_TarafHesab { get; set; }
    public long ID_tbl_Bzy { get; set; }


    public static implicit operator User(DbUser user)
    {
        return new()
        { 
            ID_tbl_Bzy = user.ID_tbl_Bzy,
            ID_tbl_TarafHesab = user.ID_tbl_TarafHesab,
            ID_tbl_Users = user.ID_tbl_Users,
            Password = user.Password,
            State_User = user.State_User,
            Username = user.Username
        };

    }
}