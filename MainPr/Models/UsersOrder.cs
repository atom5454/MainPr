using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Models
{

    public class UsersOrder
    {

        public int UsersOrderID { get; set; }

        public string UserId { get; set; }



        public User Users { get; set; }

        public ICollection<Orders> UserOrder_Orders { get; set; }

    }
}