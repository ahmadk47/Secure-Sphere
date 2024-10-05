using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SecureSphere.Models;

namespace SecureSphere.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DetectAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public DetectAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManger,
            IConfiguration configuration, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManger;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        // GET: api/DetectAPI
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Detection>>> GetDetections()
        {
            return await _context.Detections.OrderByDescending(d => d.Timestamp).ToListAsync();
        }

        // GET: api/DetectAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Detection>> GetDetection(int id)
        {
            var detection = await _context.Detections.FindAsync(id);

            if (detection == null)
            {
                return NotFound();
            }

            return detection;
        }

        // PUT: api/DetectAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetection(int id, Detection detection)
        {
            if (id != detection.ID)
            {
                return BadRequest();
            }

            _context.Entry(detection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetectionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DetectAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Detection>> PostDetection([FromBody] Detection detection)
        {
            // Check API key only for POST method
            if (!Request.Headers.TryGetValue("X-API-Key", out var apiKey) || apiKey != _configuration["APIKey"])
            {
                return Unauthorized("Invalid API Key");
            }
            // Get the current user
            //var userid = _userManager.GetUserId(User);

            // Set default values for fields not provided by the Python script
            detection.CameraID = 1; // Default camera ID
            detection.Status = 0; // Default status (e.g., 0 for "Unprocessed")
                                  // detection.UserID = userid;

            detection.UserID = null;
            
            var camera = await _context.Cameras
            .Include(c => c.Branch)
            .Include(c => c.Detections)
            .FirstOrDefaultAsync(c => c.ID == detection.CameraID);

            //var user = await _context.Users
            //.Include(u => u.Branch)
            //.Include(u => u.Detections)
            //.FirstOrDefaultAsync(u => u.Id == detection.UserID);

            if (detection.WeaponType == true)
                detection.Reason = "Knife is detected";
            else
                detection.Reason = "Gun is detected";

            _context.Detections.Add(detection);
            await _context.SaveChangesAsync();

            var receiver = "maghairehhamad@gmail.com";
            var subject = "Secure Sphere Alert - Weapon Detected";
            var message = $"{detection.Reason} at {camera.Name} {detection.Timestamp} \n" +
                          $"Location :  {camera.Branch.Address} imagepath: {detection.ImagePath} " ;

            await _emailSender.SendEmailAsync(receiver, subject, message , detection.ImagePath!);


            return CreatedAtAction(nameof(GetDetection), new { id = detection.ID }, detection);
        }

        // DELETE: api/DetectAPI/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetection(int id)
        {
            var detection = await _context.Detections.FindAsync(id);
            if (detection == null)
            {
                return NotFound();
            }

            _context.Detections.Remove(detection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetectionExists(int id)
        {
            return _context.Detections.Any(e => e.ID == id);
        }
    }
}
