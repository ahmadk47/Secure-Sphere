﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureSphere.Models;

namespace SecureSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetectAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetectAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DetectAPI
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
        [HttpPost]
        public async Task<ActionResult<Detection>> PostDetection([FromBody] Detection detection)
        {
            // Set default values for fields not provided by the Python script
            detection.CameraID = 1; // Default camera ID
            detection.Status = 0; // Default status (e.g., 0 for "Unprocessed")
            //detection.Reason = string.Empty; // Default empty reason
            detection.UserID = "4f394d62-c6c1-4604-b44d-a640fb3d6b07";
            if (detection.WeaponType == true)
                detection.Reason = "Gun is detected";
            else
                detection.Reason = "Knife is detected";

            _context.Detections.Add(detection);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetection), new { id = detection.ID }, detection);
        }

        // DELETE: api/DetectAPI/5
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