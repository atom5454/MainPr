using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Models
{

    public class Order
    {

        public int OrderID { get; set; }

        public string UserId { get; set; }



        public User Users { get; set; }

        public ICollection<Cart> Carts_Order { get; set; }

    }
}