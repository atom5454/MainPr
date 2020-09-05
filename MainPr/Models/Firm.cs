using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Models
{
    public class Firm
    {
        public int FirmID { get; set; }

        public string FirmName { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
