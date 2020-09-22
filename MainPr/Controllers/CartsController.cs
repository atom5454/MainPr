using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MainPr.Models;
using Microsoft.AspNetCore.Identity;

namespace MainPr.Controllers
{
    public class CartsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public CartsController(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        //GET: Carts
        public async Task<IActionResult> Index(string id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            id = user?.Id;

            var applicationContext = _context.Carts
                .Include(c => c.StatusCarts)
                .Include(c => c.Orders)
                .Where(c => c.Orders.UsersOrders.UserId == id);
            return View(await applicationContext.ToListAsync());
        }


        public async Task<IActionResult> AddToCartAsync(int id, string idUser)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            idUser = user?.Id;

            //Cart cart = new Cart();
            //cart.StatusCartID = 1;

            //_context.Add(cart);
            //_context.SaveChanges();

            //int idCart = cart.CartID; // Yes it's here


            var order = _context.Orders
                .Include(c => c.Items)
                .Include(c => c.UsersOrders)
                .Where(c => c.CartID == null)
                .FirstOrDefault(c => c.UsersOrders.UserId == idUser);

            return View(order);

            //return RedirectToAction(nameof(Index));
        }





        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.StatusCarts)
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartID,StatusCartID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartID,StatusCartID")] Cart cart)
        {
            if (id != cart.CartID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.StatusCarts)
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartID == id);
        }
    }
}
