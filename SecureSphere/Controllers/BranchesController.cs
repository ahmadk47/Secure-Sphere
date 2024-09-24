using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureSphere.Models;

namespace SecureSphere.Controllers
{
<<<<<<< HEAD
    [Authorize]
=======
[Authorize]
>>>>>>> ahmad1.3
    public class BranchesController : Controller
    {

        private readonly ApplicationDbContext _context;

        public BranchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index(string SearchString)
        {
                IQueryable<Branch> branches = _context.Branches.Include(b => b.Client);

                if (!string.IsNullOrEmpty(SearchString))
                {
                    branches = branches.Where(b => b.Client.Name.Contains(SearchString));
                }

                var model = await branches.ToListAsync();
                ViewBag.SearchString = SearchString;

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView(model);
                }

                return View(model);
            //if (SearchString != null)
            //{
            //    var branches = await _context.Branches.Include(b => b.Client).Where(c => c.Client.Name.Contains(SearchString)).ToListAsync();
            //    ViewBag.SearchString = SearchString;
            //    return View(branches);
            //}
            //else
            //{
            //    return View(await _context.Branches.Include(c => c.Client).ToListAsync());
            //}
            //var testDbContext = _context.Branches.Include(b => b.Client);
            //return View(await testDbContext.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var branch = await _context.Branches
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.ID == id);

            ViewBag.Cameras = _context.Cameras.Where(c => c.BranchID == id).ToList();
            ViewBag.Users = _context.Users.Where(u => u.BranchID == id).ToList();

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Branches/Create
        public IActionResult Create(int ClientID)
        {
            var branch = new Branch
            {
                ClientID = ClientID
            };

            return View(branch);
        }  

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(branch);
                await _context.SaveChangesAsync();
                ViewBag.meg = "Added Successfully";
                ViewBag.Clientlist = new SelectList(_context.Clients, "ID", "Name", branch.ClientID);
                return View();
            }
            ViewBag.Clientlist = new SelectList(_context.Clients, "ID", "Name", branch.ClientID);
            return View();


        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var branch = await _context.Branches.FindAsync(id);
                if (branch == null)
                {
                    return NotFound();
                }
                ViewBag.ClientId = new SelectList(_context.Clients, "ID", "Name", branch.ClientID);
                return View(branch);
            }

            // POST: Branches/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("ID,Address,ClientID")] Branch branch)
            {
                if (id != branch.ID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(branch);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BranchExists(branch.ID))
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
                ViewData["ClientId"] = new SelectList(_context.Clients, "ID", "ID", branch.ClientID);
                return View(branch);
            }

            // GET: Branches/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var branch = await _context.Branches
                    .Include(b => b.Client)
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (branch == null)
                {
                    return NotFound();
                }

                return View(branch);
            }

            // POST: Branches/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var branch = await _context.Branches.FindAsync(id);
                if (branch != null)
                {
                    _context.Branches.Remove(branch);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool BranchExists(int id)
            {
                return _context.Branches.Any(e => e.ID == id);
            }
    }
}
