using MainPr.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainPr.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "admin@gmail.com";
            string login = "admin";
            string user1_p = "atom54@gmail.com";
            string user2_p = "peppo@gmail.com";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, Login = login };
                User user1 = new User { Email = user1_p, UserName = user1_p, Login = "atom54" };
                User user2 = new User { Email = user2_p, UserName = user2_p, Login = "peppo" };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                await userManager.CreateAsync(user1, user1_p);
                await userManager.CreateAsync(user2, user2_p);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                    await userManager.AddToRoleAsync(user1, "user");
                    await userManager.AddToRoleAsync(user2, "user");
                }
            }
        }
    }
}