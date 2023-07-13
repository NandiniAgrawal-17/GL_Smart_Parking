using Microsoft.AspNetCore.Mvc;

namespace SmartParking.WebApp.Controllers
{
    public class AdminDashBoardController : Controller
    {
        public IActionResult AdminView()
        {
            return View();
        }


        public IActionResult PortalLandingPage()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }
    }
}
