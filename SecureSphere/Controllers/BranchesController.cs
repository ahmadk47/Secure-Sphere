﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SecureSphere.Models;

namespace SecureSphere.Controllers
{

    [Authorize]

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
            await Logger.LogAsync($"User requested Index For Branches ", _context);
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
            await Logger.LogAsync($"User '{_context.Branches.Include(b => b.Client.Name)}' searched for '{SearchString}' ", _context);
            
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
            await Logger.LogAsync($"User requested Details For Branches ", _context);
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
            Logger.LogAsync($"User requested Create For Branches ", _context);
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
            await Logger.LogAsync($"User requested Create confirmed For Branches ", _context);
            if (ModelState.IsValid)
            {
                _context.Add(branch);
                await _context.SaveChangesAsync();
                ViewBag.meg = "Added Successfully";
                //ViewBag.Clientlist = new SelectList(_context.Clients, "ID", "Name", branch.ClientID);
                return View(branch);
            }
            ViewBag.Clientlist = new SelectList(_context.Clients, "ID", "Name", branch.ClientID);
            return View();


        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? id)
            {
            await Logger.LogAsync($"User requested Edit For Branches ", _context);
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
            await Logger.LogAsync($"User requested Edit confirmed For Branches ", _context);
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
            await Logger.LogAsync($"User requested Delete For Branches ", _context);
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
            await Logger.LogAsync($"User requested Delete confirmed For Branches ", _context);
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
