using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MainPr.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MainPr.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public ItemsController(ApplicationContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Items.Include(i => i.Firms);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Firms)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,ItemName,Title,Price,FirmID")] Item item, IFormFile titleImageFile)
        {
            if (ModelState.IsValid)
            {

                if (titleImageFile != null)
                {
                    item.ImgPath = titleImageFile.FileName;
                    using (var stream = new FileStream(Path.Combine(hostEnvironment.WebRootPath, "img/", titleImageFile.FileName), FileMode.Create))
                    {
                        titleImageFile.CopyTo(stream);
                    }
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentsDropDownList(item.FirmID);
            return View(item);
        }


        private void PopulateDepartmentsDropDownList(object selectedFirm = null)
        {
            var departmentsQuery = from d in _context.Firms
                                   orderby d.FirmName
                                   select d;
            ViewBag.FirmID = new SelectList(departmentsQuery.AsNoTracking(), "FirmID", "FirmName", selectedFirm);
        }



        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["FirmID"] = new SelectList(_context.Firms, "FirmID", "FirmID", item.FirmID);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,ItemName,Title,Price,FirmID,ImgPath")] Item item, IFormFile titleImageFile)
        {
            if (id != item.ItemID)
            {
                return NotFound();
            }

            if (titleImageFile != null)
            {
                item.ImgPath = titleImageFile.FileName;
                using (var stream = new FileStream(Path.Combine(hostEnvironment.WebRootPath, "img/", titleImageFile.FileName), FileMode.Create))
                {
                    titleImageFile.CopyTo(stream);
                }
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemID))
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
            ViewData["FirmID"] = new SelectList(_context.Firms, "FirmID", "FirmID", item.FirmID);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Firms)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemID == id);
        }
    }
}
