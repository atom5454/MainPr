using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Models
{
    public class StatusOrder
    {
        public int StatusOrderID { get; set; }

        public string StatusName { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
