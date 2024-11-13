using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infrastructure.Data
{
    public class ConnectDbContext : IdentityDbContext<SystemUser, IdentityRole, string, 
        IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ConnectDbContext (DbContextOptions<ConnectDbContext> dbOptions) : base(dbOptions) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
