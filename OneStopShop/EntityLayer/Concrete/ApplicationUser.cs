﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class ApplicationUser: IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string? Image { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public IList<string> Roles { get; set; }
    }
}
