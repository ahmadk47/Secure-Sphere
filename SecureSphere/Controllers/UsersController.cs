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

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
                users = users.Where(b => b.Branch.Client.Name.Contains(SearchString));
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
        // GET: Users/Create
        public async Task<IActionResult> Create(int BranchID,string Name)
        {
            await Logger.LogAsync($"User requested Create For Users ", _context);

            var user = new ApplicationUser
            {
                BranchID = BranchID,
            };
            ViewData["Roles"] = new SelectList(_context.Roles.ToList(), "Name", "Name"); // Get available roles
            return View(user);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user, string selectedRole)
        {
            await Logger.LogAsync($"User requested Create For Users ", _context);

            // Create the user
            var result = await _userManager.CreateAsync(user, user.PasswordHash!);

            if (result.Succeeded)
            {
                // Assign the role to the user
                await _userManager.AddToRoleAsync(user, selectedRole);
                ViewBag.meg = "Added Successfully";
                return View(user);
            }

            // If there are errors, display them
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            ViewData["Roles"] = new SelectList(_context.Roles.ToList(), "Name", "Name"); // Reload roles if something fails
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
            //ViewData["Roles"] = new SelectList(_context.Roles.ToList(), "Id", "Name");
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address", user.BranchID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, ApplicationUser user)
        //{
        //    await Logger.LogAsync($"User requested Edit For Users ", _context);
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //_context.Update(user);
        //            await _userManager.UpdateAsync(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));

        //    }
        //    //ViewData["Roles"] = new SelectList(_context.Roles.ToList(), "Id", "Name");
        //    ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address", user.BranchID);
        //    return View(user);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser userModel)
        {
            await Logger.LogAsync($"User requested Edit For Users ", _context);
            if (id != userModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing user from the database
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing user
                    user.UserName = userModel.UserName;
                    user.Email = userModel.Email;
                    user.BranchID = userModel.BranchID;
                    user.CreatedAt = userModel.CreatedAt;
                    // Update other properties as needed

                    // Use UserManager to update the user
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return View(user);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address", userModel.BranchID);
            return View(userModel);
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