using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.ViewModels
{
    public class EditUserViewModel
    {
        public string Email { get; set; }
        
        public string Login { get; set; }

        public string PhoneNumber { get; set; }

        public string NormalizedUserName { get; set; }
    }
}
