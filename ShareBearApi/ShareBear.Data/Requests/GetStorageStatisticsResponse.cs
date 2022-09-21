using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBear.Data.Requests
{
    public class GetStorageStatisticsResponse
    {
        public double MaxStorageBytes{ get; set; }
        public double UsedStorageBytes { get; set; }
        public bool HasFreeSpace { get; set; }
    }
}
