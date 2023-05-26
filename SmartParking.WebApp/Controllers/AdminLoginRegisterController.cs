using Microsoft.AspNetCore.Mvc;

namespace SmartParking.WebApp.Controllers
{
    public class AdminLoginRegisterController : Controller
    {
        public IActionResult AdminLogin()
        {
            return View();
        }
        public IActionResult AdminRegister()
        {
            return View();
        }
    }
}
