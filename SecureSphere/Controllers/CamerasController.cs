﻿using System;
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
    [Authorize]
    public class CamerasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CamerasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cameras
        public async Task<IActionResult> Index(string SearchString)
        {
            await Logger.LogAsync($"User requested Index For Cameras ", _context);
            IQueryable<Camera> cameras = _context.Cameras.Include(b => b.Branch.Cameras);

            if (!string.IsNullOrEmpty(SearchString))
            {
                cameras = cameras.Where(b => b.Branch.Client.Name.Contains(SearchString));
            }

            var model = await cameras.ToListAsync();
            ViewBag.SearchString = SearchString;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(model);
            }

            return View(model);
        }

        // GET: Cameras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            await Logger.LogAsync($"User requested Details For Cameras ", _context);
            if (id == null)
            {
                return NotFound();
            }

            var camera = await _context.Cameras
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.ID == id);

            ViewBag.Detections = _context.Detections.Where(d => d.CameraID == id).ToList();

            if (camera == null)
            {
                return NotFound();
            }

            return View(camera);
        }

        // GET: Cameras/Create
        public IActionResult Create(int BranchID)
        {
            Logger.LogAsync($"User requested Create For Cameras ", _context);
            var camera = new Camera
            {
                BranchID = BranchID
            }; 
           //ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address");
            return View(camera);
        }

        // POST: Cameras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,BranchID,IpAddress,Status")] Camera camera)
        {
            await Logger.LogAsync($"User requested Create confirmed For Cameras ", _context);
            if (ModelState.IsValid)
            {
                _context.Add(camera);
                await _context.SaveChangesAsync();
                //ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address", camera.Branch.Address);
                return View(camera);
            }
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address", camera.Branch.Address);
            return View();
        }

        // GET: Cameras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            await Logger.LogAsync($"User requested Edit For Cameras ", _context);
            if (id == null)
            {
                return NotFound();
            }

            var camera = await _context.Cameras.FindAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address", camera.BranchID);
            return View(camera);
        }

        // POST: Cameras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,BranchID,IpAddress,Status")] Camera camera)
        {
            await Logger.LogAsync($"User requested Edit For Cameras ", _context);
            if (id != camera.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CameraExists(camera.ID))
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
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "Address", camera.BranchID);
            return View(camera);
        }

        // GET: Cameras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            await Logger.LogAsync($"User requested Delete For Cameras ", _context);
            if (id == null)
            {
                return NotFound();
            }

            var camera = await _context.Cameras
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (camera == null)
            {
                return NotFound();
            }

            return View(camera);
        }

        // POST: Cameras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Logger.LogAsync($"User requested Delete Confirmed For Cameras ", _context);
            var camera = await _context.Cameras.FindAsync(id);
            if (camera != null)
            {
                _context.Cameras.Remove(camera);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CameraExists(int id)
        {
            return _context.Cameras.Any(e => e.ID == id);
        }
    }
}
