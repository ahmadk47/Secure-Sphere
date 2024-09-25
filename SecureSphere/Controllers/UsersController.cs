using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureSphere.Models;


namespace SecureSphereApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index(string SearchString)
        {
            await Logger.LogAsync($"User requested Index For Users ", _context);
            IQueryable<ApplicationUser> users = _context.Users.Include(b => b.Branch);

            if (!string.IsNullOrEmpty(SearchString))
            {
                users =users.Where(b => b.Branch.Client.Name.Contains(SearchString));
                await Logger.LogAsync($"User Searched For '{SearchString}' ", _context);
            }

            var model = await users.ToListAsync();
            ViewBag.SearchString = SearchString;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(model);
            }

            return View(model);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            await Logger.LogAsync($"User requested Details For Users ", _context);
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewBag.Detections = _context.Detections.Where(u => u.UserID == id).ToList();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create(int BranchID)
        {
             Logger.LogAsync($"User requested Create For Users ", _context);
            var user = new ApplicationUser
            {
                BranchID = BranchID
            };
            //ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address");
            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            await Logger.LogAsync($"User requested Create For Users ", _context);
            await _userManager.CreateAsync(user,user.PasswordHash!);
            //if (ModelState.IsValid)
            //{
            //    //user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            //    _context.Add(user);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "ID", user.BranchID);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            await Logger.LogAsync($"User requested Edit For Users ", _context);
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "ID", user.BranchID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Email,Password,CreateAt,BranchID")] ApplicationUser user)
        {
            await Logger.LogAsync($"User requested Edit For Users ", _context);
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "ID", user.BranchID);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            await Logger.LogAsync($"User requested Delete For Users ", _context);
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await Logger.LogAsync($"User requested Delete Confirmed For Users ", _context);
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}