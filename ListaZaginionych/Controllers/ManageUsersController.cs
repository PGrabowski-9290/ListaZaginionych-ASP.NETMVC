using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ListaZaginionych.Models;
using System.Linq;

namespace ListaZaginionych.Controllers
{
    [Authorize(Policy = "writepolicy")]
    public class ManageUsersController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public ManageUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersViewModel = new List<UserViewModel>();
            foreach (var user in users)
            {
                var thisViewModel = new UserViewModel();
                thisViewModel.Id = user.Id;
                thisViewModel.UserName = user.UserName;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUsersRoles(user);
                usersViewModel.Add(thisViewModel);
            }
            return View(usersViewModel);
        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Nie odnalezion użytkownika o Id";              
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoles(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Nie można usunąć obecnych ról");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Nie można dodać zaznaczonych ról");
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var thisViewModel = new UserViewModel();
            thisViewModel.Id = user.Id;
            thisViewModel.UserName = user.UserName;
            thisViewModel.Email = user.Email;
            thisViewModel.Roles = await GetUsersRoles(user);
            
            return View(thisViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.UserName = model.UserName;
            user.Email = model.Email;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Błąd przy aktualizacji Użytkownika");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<string>> GetUsersRoles(IdentityUser user)
        {
            return new List<string> (await _userManager.GetRolesAsync(user));
        }

    }
}
