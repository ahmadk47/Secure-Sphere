using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureSphere.Models;
using System.Security.Claims;

namespace SecureSphere.Controllers
{
    [Authorize]
    public class DetectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public DetectionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Detections
        public async Task<IActionResult> Index()
        {
            await Logger.LogAsync($"User requested Index For Detections ", _context);
            var testDbContext = _context.Detections.Include(d => d.Camera).Include(d => d.User);
            return View(await testDbContext.ToListAsync());
        }

        // GET: Detections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            await Logger.LogAsync($"User requested Details For Detections ", _context);
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
        [Authorize]
        public async Task<IActionResult> ResponsibleUser(int? id)
        {
            await Logger.LogAsync($"User requested Details For Detections ", _context);
            if (id == null)
            {
                return NotFound();
            }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var detect = await _context.Detections.FirstOrDefaultAsync(d => d.ID == id);
            detect!.UserID = userid;
            _context.Update(detect);
            await _context.SaveChangesAsync();
            return View();
        }


        // GET: Detections/Create
        public IActionResult Create()
        {
            Logger.LogAsync($"User requested Create For Detections ", _context);
            ViewData["CameraID"] = new SelectList(_context.Cameras, "ID", "Name");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Detections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CameraID,Timestamp,WeaponType,Confidence,Status,Reason,UserID")] Detection detection)
        {
            await Logger.LogAsync($"User requested Create Confirmed For Detections ", _context);
            if (ModelState.IsValid)
            {
                _context.Add(detection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CameraID"] = new SelectList(_context.Cameras, "ID", "Name", detection.CameraID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "UserName", detection.UserID);
            return View(detection);
        }

        // GET: Detections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            await Logger.LogAsync($"User requested Edit For Detections ", _context);
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "UserName", detection.UserID);
            return View(detection);
        }

        // POST: Detections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CameraID,Timestamp,WeaponType,Confidence,Status,Reason,UserID")] Detection detection)
        {
            await Logger.LogAsync($"User requested Edit For Detections ", _context);
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "UserName", detection.UserID);
            return View(detection);
        }

        // GET: Detections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            await Logger.LogAsync($"User requested Delete For Detections ", _context);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Logger.LogAsync($"User requested Delete Confirmed For Detections ", _context);
            var detection = await _context.Detections.FindAsync(id);
            if (detection != null)
            {
                _context.Detections.Remove(detection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetectionExists(int id)
        {
            return _context.Detections.Any(e => e.ID == id);
        }
    }
}
