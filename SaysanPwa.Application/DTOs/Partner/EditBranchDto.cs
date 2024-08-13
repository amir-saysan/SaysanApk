
namespace SaysanPwa.Application.DTOs.Partner
{
    public class EditBranchDto
    {
        public long? ID_tbl_Partner_Branch { get; set; }
        public string? Name_responsible { get; set; }
        public string? Name_Branch { get; set; }
        public bool State_Branch { get; set; } = true;
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public int? ID_tbl_Ostan { get; set; }
        public int? ID_tbl_SharOstan { get; set; }
        public string? CodePosti { get; set; }
        public string? Tell { get; set; }
        public string? Address { get; set; }
        public string? Location_Branch { get; set; }

    }
}
