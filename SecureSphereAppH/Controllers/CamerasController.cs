using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureSphereApp.Models;

namespace SecureSphereApp.Controllers
{
    public class CamerasController : Controller
    {
        private readonly TestDbContext _context;

        public CamerasController(TestDbContext context)
        {
            _context = context;
        }

        // GET: Cameras
        public async Task<IActionResult> Index()
        {
            var testDbContext = _context.Cameras.Include(c => c.Branch);
            return View(await testDbContext.ToListAsync());
        }

        // GET: Cameras/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camera = await _context.Cameras
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camera == null)
            {
                return NotFound();
            }

            return View(camera);
        }

        // GET: Cameras/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id");
            return View();
        }

        // POST: Cameras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BranchId,IpAddress,Status")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                _context.Add(camera);
                await _context.SaveChangesAsync();
                ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", camera.BranchId);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", camera.BranchId);
            return View();
        }

        // GET: Cameras/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camera = await _context.Cameras.FindAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", camera.BranchId);
            return View(camera);
        }

        // POST: Cameras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Name,BranchId,IpAddress,Status")] Camera camera)
        {
            if (id != camera.Id)
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
                    if (!CameraExists(camera.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", camera.BranchId);
            return View(camera);
        }

        // GET: Cameras/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camera = await _context.Cameras
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camera == null)
            {
                return NotFound();
            }

            return View(camera);
        }

        // POST: Cameras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera != null)
            {
                _context.Cameras.Remove(camera);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CameraExists(decimal id)
        {
            return _context.Cameras.Any(e => e.Id == id);
        }
    }
}
