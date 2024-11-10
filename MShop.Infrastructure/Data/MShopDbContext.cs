using Microsoft.EntityFrameworkCore;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infrastructure.Data
{
    public class MShopDbContext : DbContext
    {
        public MShopDbContext(DbContextOptions<MShopDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}
