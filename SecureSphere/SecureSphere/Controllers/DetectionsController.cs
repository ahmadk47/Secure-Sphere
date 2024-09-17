using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureSphere.Models;

namespace SecureSphere.Controllers
{
    public class DetectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetectionsController(ApplicationDbContext context)
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
                .FirstOrDefaultAsync(m => m.ID == id);
            if (detection == null)
            {
                return NotFound();
            }

            return View(detection);
        }

        // GET: Detections/Create
        public IActionResult Create()
        {
            ViewData["CameraID"] = new SelectList(_context.Cameras, "ID", "Name");
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "Name");
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
            ViewData["CameraID"] = new SelectList(_context.Cameras, "ID", "Name", detection.CameraID);
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "Name", detection.UserID);
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
            ViewData["CameraID"] = new SelectList(_context.Cameras, "ID", "Name", detection.CameraID);
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "Name", detection.UserID);
            return View(detection);
        }

        // POST: Detections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,CameraId,Timestamp,WeaponType,Confidence,Status,Reason,UserId")] Detection detection)
        {
            if (id != detection.ID)
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
                    if (!DetectionExists(detection.ID))
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
            ViewData["CameraID"] = new SelectList(_context.Cameras, "ID", "Name", detection.CameraID);
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "Name", detection.UserID);
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
                .FirstOrDefaultAsync(m => m.ID == id);
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
            return _context.Detections.Any(e => e.ID == id);
        }
    }
}
