using SaysanPwa.Domain.SeedWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaysanPwa.Domain.AggregateModels.LocationAggregate
{
    public class City : Entity, IAggregateRoot
    {
        public int ID_tbl_SharOstan { get; set; }
        public string Name_SharOstan { get; set; } = string.Empty;
        public string Code_Shahrostan { get; set; } = string.Empty;
        public string ID_tbl_Ostan { get; set; } = string.Empty;
    }
}




