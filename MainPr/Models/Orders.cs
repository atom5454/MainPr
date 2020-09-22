using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;


namespace MainPr.Models
{
    public class Orders
    {
        public int ItemID { get; set; }
        public int UsersOrderID { get; set; }


        public int? CartID { get; set; }


        public int CountBuy_item { get; set; }
        public double Price { get; set; }

        public Item Items { get; set; }
        public UsersOrder UsersOrders { get; set; }

        public ICollection<Cart> Carts { get; set; }

    }
}