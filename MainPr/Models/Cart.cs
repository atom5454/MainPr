using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Models
{
    public class Cart
    {
        public int ItemID { get; set; }
        public int OrderID { get; set; }

        public int Count_item { get; set; }
        public double Price { get; set; }


        public int StatusCartID { get; set; }

        public StatusCart StatusCarts { get; set; }
    }
}
