using System.Collections.Generic;

namespace MainPr.Models
{
    public class Firm
    {
        public int FirmID { get; set; }

        public string FirmName { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
