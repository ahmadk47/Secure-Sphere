using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SecureSphere.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SecureSphere.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment; // To get the wwwroot path

        public ProfilesController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        // Action to display the profile page
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Pass the user to the view
        }

        // Action to edit the profile (GET)
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Pass the user to the view
        }

        // Action to edit the profile (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                // Update the user profile fields
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber; // Update other fields as necessary

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                // Add errors to ModelState if the update failed
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // 1. Upload Profile Picture
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                // Get the path to wwwroot folder
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/profile_pictures");

                // Ensure directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create a unique file name for the uploaded picture
                var uniqueFileName = $"{user.Id}_{Path.GetFileName(profilePicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                // Update user's profile picture URL in the database
                user.ProfilePictureUrl = $"/uploads/profile_pictures/{uniqueFileName}";
                await _userManager.UpdateAsync(user);

                return Ok(new { success = true, message = "Profile picture uploaded successfully!" });
            }

            return BadRequest(new { success = false, message = "Invalid profile picture." });
        }


        // 3. Change Password
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                // Change the password
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    // Refresh sign-in to ensure the user is authenticated with the new password
                    await _signInManager.RefreshSignInAsync(user);
                    return Ok(new { success = true, message = "Password changed successfully!" });
                }

                return BadRequest(new { success = false, errors = result.Errors });
            }

            return BadRequest(new { success = false, message = "Invalid data." });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveProfilePicture()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Set to a default profile picture path
            user.ProfilePictureUrl = ""; // Adjust the path accordingly

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Profile picture removed successfully." });
            }

            return BadRequest("Failed to remove profile picture.");
        }

    }
}
