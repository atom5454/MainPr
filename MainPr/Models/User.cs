﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MainPr.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }

        public ICollection<UsersOrder> Orders { get; set; }
    }
}