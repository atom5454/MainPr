using Microsoft.AspNetCore.Identity;
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Firm>().HasData(
                new Firm
                {
                    FirmID = 1,
                    FirmName = "SpaceX"
                },
                new Firm
                {
                    FirmID = 2,
                    FirmName = "KSP"
                });
            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    ItemID = 1,
                    ItemName = "Car",
                    Title = "Best car",
                    Price = 150,
                    ImgPath = "8ipwnn.jpg",
                    CountItems = 50,
                    FirmID = 1,
                },
                new Item
                {
                    ItemID = 2,
                    ItemName = "Plane",
                    Title = "Best plane",
                    Price = 200,
                    ImgPath = "Morning_Rays.jpg",
                    CountItems = 50,
                    FirmID = 1,
                },
                new Item
                {
                    ItemID = 3,
                    ItemName = "Car",
                    Title = "Best car",
                    Price = 300,
                    ImgPath = "priroda_gory_nebo_ozero_oblaka_81150_1920x1080.jpg",
                    CountItems = 50,
                    FirmID = 2,
                });

            modelBuilder.Entity<StatusCart>().HasData(
                new StatusCart
                {
                    StatusCartID = 1,
                    StatusName = "In processing",
                },
                new StatusCart
                {
                    StatusCartID = 2,
                    StatusName = "Accepted",
                },
                new StatusCart
                {
                    StatusCartID = 3,
                    StatusName = "Rejected",
                });
            modelBuilder.Entity<StatusOrder>().HasData(
                new StatusOrder
                {
                    StatusOrderID = 1,
                    StatusName = "Wait"
                },
                new StatusOrder
                {
                    StatusOrderID = 2,
                    StatusName = "Accepted"
                });


            modelBuilder.Entity<Orders>()
                .HasKey(c => new {c.ItemID, c.UsersOrderID });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cart>()
                .HasKey(c => new { c.CartID, c.ItemID, c.UsersOrderID });
            base.OnModelCreating(modelBuilder);
        }
    }

}