using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAnnuaire.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace WebApplicationAnnuaire.Controllers
{
    [Authorize(Roles = "Administration")]
    public class UsersController : Controller
    {
       private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var allRoles = _context.Roles.Select(r => r.Name).ToList();
            ViewBag.AllRoles = allRoles;
            var users = _userManager.Users.ToList();
            var rolesByUser = new Dictionary<string, List<string>>();
            foreach (var user in users)
            {
                var roles = _userManager.GetRolesAsync(user).Result;
                rolesByUser[user.Id] = roles.ToList();
            }
            ViewBag.Users = users;
            ViewBag.RolesByUser = rolesByUser;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateRoles(string userId, string[] roles)
        {
            // Retrieve the user based on the ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Remove any existing roles
            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            // Add the new roles
            var result = await _userManager.AddToRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update roles");
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove role");
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to delete user");
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }

    }
}
