﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MainPr.Models;
using Microsoft.AspNetCore.Identity;
using MainPr.ViewModels;

namespace MainPr.Controllers
{
    public class PAccController : Controller
    {

        //private UserManager<User> _userManager;
        private UserManager<User> _userManager;
        
        public PAccController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        //public async Task<IActionResult> AccE(User userdet)
        //{
        //    IdentityResult x = await _userManager.UpdateAsync(userdet);
        //    if (x.Succeeded)
        //    {
        //        return RedirectToAction("Privacy", "Home");
        //    }
        //    return View(userdet);
        //}

        public IActionResult Acc()
        {
            var userid = _userManager.GetUserId(User);
            User user = _userManager.FindByIdAsync(userid).Result;

            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            var userid = _userManager.GetUserId(User);
        
            User user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel {Email = user.Email, Login = user.Login, PhoneNumber = user.PhoneNumber, NormalizedUserName = user.NormalizedUserName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userid = _userManager.GetUserId(User);
                User user = await _userManager.FindByIdAsync(userid);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.Login = model.Login;
                    user.PhoneNumber = model.PhoneNumber;
                    user.NormalizedUserName = model.NormalizedUserName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Acc", "PAcc");
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


    }
}