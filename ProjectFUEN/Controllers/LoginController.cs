using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectFUEN.Models.ViewModels;
using ProjectFUEN.Models.EFModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ProjectFUEN.Controllers
{
    public class LoginController : Controller
    {

        private readonly ProjectFUENContext _context;

        public LoginController(ProjectFUENContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM request)
        {
            var admin = await _context.Administrators.FindAsync(request.Account);

            if (admin != null && request.Password == admin.Password)
                {
                    // TODO SignIn
                    var cliams = new List<Claim>()
                    {
                    new Claim(ClaimTypes.Name, request.Account),
                    new Claim("FullName", "Fish"),
                    new Claim(ClaimTypes.Role, "Administrator")
                    };

                    var claimIdentity = new ClaimsIdentity(cliams, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 放入Cookie 細節
                    var authProperties = new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }

            ModelState.AddModelError("Password", "帳號或密碼錯誤");
    
            return View(request);
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
