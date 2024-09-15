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
    public class DetectionsController : Controller
    {
        private readonly TestDbContext _context;

        public DetectionsController(TestDbContext context)
        {
            _context = context;
        }

        // GET: Detections
        public async Task<IActionResult> Index()
        {
            var testDbContext = _context.Detections.Include(d => d.Camera).Include(d => d.User);
            return View(await testDbContext.ToListAsync());
        }

        // GET: Detections/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detection = await _context.Detections
                .Include(d => d.Camera)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detection == null)
            {
                return NotFound();
            }

            return View(detection);
        }

        // GET: Detections/Create
        public IActionResult Create()
        {
            ViewData["CameraId"] = new SelectList(_context.Cameras, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Detections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CameraId,Timestamp,WeaponType,Confidence,Status,Reason,UserId")] Detection detection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CameraId"] = new SelectList(_context.Cameras, "Id", "Name", detection.CameraId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", detection.UserId);
            return View(detection);
        }

        // GET: Detections/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detection = await _context.Detections.FindAsync(id);
            if (detection == null)
            {
                return NotFound();
            }
            ViewData["CameraId"] = new SelectList(_context.Cameras, "Id", "Name", detection.CameraId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", detection.UserId);
            return View(detection);
        }

        // POST: Detections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,CameraId,Timestamp,WeaponType,Confidence,Status,Reason,UserId")] Detection detection)
        {
            if (id != detection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetectionExists(detection.Id))
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
            ViewData["CameraId"] = new SelectList(_context.Cameras, "Id", "Name", detection.CameraId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", detection.UserId);
            return View(detection);
        }

        // GET: Detections/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detection = await _context.Detections
                .Include(d => d.Camera)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detection == null)
            {
                return NotFound();
            }

            return View(detection);
        }

        // POST: Detections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var detection = await _context.Detections.FindAsync(id);
            if (detection != null)
            {
                _context.Detections.Remove(detection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetectionExists(decimal id)
        {
            return _context.Detections.Any(e => e.Id == id);
        }
    }
}
