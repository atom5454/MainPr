﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainPr.Models;
using MainPr.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainPr.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public AdminController(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
        public async Task<IActionResult> AcceptCart(string id)
        {
            var applicationContext = _context.Carts
                .Include(c => c.StatusCarts)
                .Include(c => c.Orders.Items)
                .Include(c => c.Orders);
            return View(await applicationContext.ToListAsync());
        }

        public IActionResult All_Users()
        {
            return View(_userManager.Users.ToList());
        }


        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Login = model.Login };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Login = user.Login, PhoneNumber = user.PhoneNumber, NormalizedUserName = user.NormalizedUserName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.Login = model.Login;
                    user.PhoneNumber = model.PhoneNumber;
                    user.NormalizedUserName = model.NormalizedUserName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("All_Users");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}