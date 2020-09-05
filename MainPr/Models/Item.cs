using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Models
{
    public class Item
    {
        public int ItemID { get; set; }

        public string ItemName { get; set; }

        public string Title { get; set; }

        public float Price { get; set; }

        public string ImgPath { get; set; }




        public int FirmID { get; set; }

        public Firm Firms { get; set; }
    }
}
