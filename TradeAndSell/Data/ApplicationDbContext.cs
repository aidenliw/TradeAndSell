using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradeAndSell.Models;

namespace TradeAndSell.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TradeAndSell.Models.Item> Item { get; set; }
        public DbSet<TradeAndSell.Models.Order> Order { get; set; }
        public DbSet<TradeAndSell.Models.Cart> Cart { get; set; }
    }
}
