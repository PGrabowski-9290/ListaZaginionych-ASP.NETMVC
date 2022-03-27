using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ListaZaginionych.Controllers
{
    [Authorize(Policy = "writepolicy")]
    public class ManageUsersController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public IActionResult Index()
        {
            return View();
        }
    }
}
