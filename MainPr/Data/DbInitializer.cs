using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainPr.Models;
using MainPr.Data;
using Microsoft.AspNetCore.Identity;

namespace MainPr.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureCreated();

            if (context.Items.Any())
            {
                return;   
            }

            var firms = new Firm[]
            {
                new Firm
                {
                    FirmID = 1,
                    FirmName = "SpaceX"
                },
                new Firm
                {
                    FirmID = 2,
                    FirmName = "KSP"
                }
            };

            foreach (Firm i in firms)
            {
                context.Firms.Add(i);
            }
            context.SaveChanges();

            var items = new Item[]
            {
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
                }
            };

            foreach (Item s in items)
            {
                context.Items.Add(s);
            }
            context.SaveChanges();


            var users = new User[]
            {
                new User
                {
                    Id = "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                    UserName = "atom54@gmail.com",
                    NormalizedUserName = "ATOM54@GMAIL.COM",
                    Email = "atom54@gmail.com",
                    NormalizedEmail = "ATOM54@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "atom54@gmail.com"),
                    SecurityStamp = string.Empty,
                    Login = "atom54",
                },
                new User
                {
                    Id = "3b62472e-4f66-49fa-a20f-e7685b9125d8",
                    UserName = "peppo@gmail.com",
                    NormalizedUserName = "PEPPO@GMAIL.COM",
                    Email = "peppo@gmail.com",
                    NormalizedEmail = "PEPPO@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "peppo@gmail.com"),
                    SecurityStamp = string.Empty,
                    Login = "peppo",
                }
            };

            foreach (User d in users)
            {
                context.Users.Add(d);
            }
            context.SaveChanges();

            var status = new StatusCart[]
            {
                new StatusCart
                {
                    StatusCartID = 1,
                    StatusName = "In processing",
                },
                new StatusCart
                {
                    StatusCartID = 2,
                    StatusName = "Accepted",
                }
            };

            foreach (StatusCart c in status)
            {
                context.StatusCarts.Add(c);
            }
            context.SaveChanges();

        }
    }
}