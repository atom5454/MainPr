using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Models
{
    public class Cart
    {
        public int CartID { get; set; }

        public int ItemID { get; set; }
        public int UsersOrderID { get; set; }


        public int StatusCartID { get; set; }


        public StatusCart StatusCarts { get; set; }
        public Orders Orders { get; set; }
    }
}
