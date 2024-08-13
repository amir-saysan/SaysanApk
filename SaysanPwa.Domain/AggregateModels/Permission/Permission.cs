using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.Permission;

public class Permission : Entity, IAggregateRoot
{
	public int ID_tbl_Users { get; set; }
	public string UserPermission { get; set; }


}

