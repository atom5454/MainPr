using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Models
{
    public class StatusCart
    {
        public int StatusCartID { get; set; }

        public string StatusName { get; set; }


        public ICollection<Cart> Carts { get; set; }
    }
}