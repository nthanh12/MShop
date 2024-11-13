using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Domain.Entities
{
    public class SystemUser : IdentityUser
    {
        public string Name { get; set; }
        public string Phone {  get; set; }
        public string Address { get; set; }
    }
}
