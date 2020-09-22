using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainPr.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Firm> Firms { get; set; }
        public DbSet<UsersOrder> UsersOrders { get; set; }
        public DbSet<StatusCart> StatusCarts { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<Firm>().ToTable("Firm");
            modelBuilder.Entity<User>().ToTable("AspNetUsers");

            modelBuilder.Entity<StatusCart>().ToTable("StatusCart");



            modelBuilder.Entity<Orders>()
                .HasKey(c => new {c.ItemID, c.UsersOrderID });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cart>()
                .HasKey(c => new { c.CartID, c.ItemID, c.UsersOrderID });
            base.OnModelCreating(modelBuilder);
        }
    }

}