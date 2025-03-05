using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CapstoneProjectManagementSystemV3UI.Controllers.Staff_Controlller
{
    //[Authorize(Roles ="Staff")]
    public class LoginAsController : Controller
    {
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly IConfiguration _configuration;

        public LoginAsController(
                                ISessionExtensionService sessionExtensionService
            , IConfiguration configuration
            )
        {
            _sessionExtensionService = sessionExtensionService;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View("/Views/Staff_View/LoginAs/Index.cshtml");
        }
        [HttpPost]
        public async Task<JsonResult> LoginAsAsync([FromBody] string fptEmail)
        {
            try
            {
                if (fptEmail == null || fptEmail == "")
                {
                    return Json(0);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                        string apiUrl = "/api/LoginAs/LoginAsAsync";
                        HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, new { fptEmail });
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var user = JsonConvert.DeserializeObject<User>(jsonString);
                            if (user != null)
                            {
                                HttpContext.Session.Remove("sessionAccount");
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "sessionAccount", user);
                                return Json(1);
                            }
                            else
                            {
                                return Json(2);
                            }
                        }
                        else
                        {
                            return Json(2);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(0);
            }
        }
    }
}
