using SaysanPwa.Domain.SeedWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaysanPwa.Domain.AggregateModels.LocationAggregate
{
    public class Region: Entity, IAggregateRoot
    {
        public int ID_tbl_Ostan { get; set; }
        public string Name_Ostan { get; set; } = null!;
        public string Code_Ostan { get; set; } = null!;
    }
}


