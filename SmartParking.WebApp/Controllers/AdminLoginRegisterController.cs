using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartParking.WebApp.Models.AdminGetDetails;
using System.Net.Http.Json;

namespace SmartParking.WebApp.Controllers
{
    public class AdminLoginRegisterController : Controller
    {

        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdminLogin(AdminLoginRequest adminlogin)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7258/api/AdminAuthentication/LoginAdmin");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<AdminLoginRequest>("AdminLogin",adminlogin);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("PortalLandingPage");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(adminlogin);
            /*AdminLoginResponse adminLoginResponse;
            IEnumerable<AdminLoginRequest> adminLoginRequests;
            using (var httpClient = new HttpClient())//handler
            {
                using (var response = await httpClient.GetAsync("https://localhost:7258/api/AdminAuthentication/LoginAdmin"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    adminLoginResponse = JsonConvert.DeserializeObject<AdminLoginResponse>(apiResponse);
                    adminLoginRequests = adminLoginResponse.AdminLoginAsync();
                }
            }
            return View(adminLoginRequests);*/
        }

        public IActionResult AdminRegister()
        {
            return View();
        }
    }
}
