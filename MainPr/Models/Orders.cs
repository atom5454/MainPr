using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MainPr.Models
{
    public class Orders
    {
        public int ItemID { get; set; }
        public int UsersOrderID { get; set; }

        public int StatusOrderID { get; set; }

        [Range(0, 5)]
        public int CountBuy_item { get; set; }
        public double Price { get; set; }


        public Item Items { get; set; }
        public UsersOrder UsersOrders { get; set; }
        public StatusOrder StatusOrder { get; set;  }
        public ICollection<Cart> Carts { get; set; }

    }
}