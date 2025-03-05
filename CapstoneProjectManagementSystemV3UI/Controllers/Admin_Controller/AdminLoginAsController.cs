using DocumentFormat.OpenXml.Wordprocessing;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace CapstoneProjectManagementSystem.Controllers.Admin_Controller
{
    //[Authorize(Roles = "Admin")]
    public class AdminLoginAsController : Controller
    {
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly ILogger<AdminLoginAsController> _logger;
        private readonly IConfiguration _configuration;


        public AdminLoginAsController(ISessionExtensionService sessionExtensionService
            , IConfiguration configuration
                                , ILogger<AdminLoginAsController> logger)
        {
            _sessionExtensionService = sessionExtensionService;
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("View loginAs screen");
                return View("/Views/Admin_View/LoginAs/Index.cshtml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "View loginAs screen error");
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }

        [HttpPost]
        public async Task<JsonResult> LoginAs([FromBody] string fptEmail)
        {
            try
            {
                if (fptEmail == null || fptEmail == "")
                {
                    return Json(false);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                        string apiUrl = "/api/admin/login-as";
                        client.DefaultRequestHeaders.Add("X-Connection-String", "HL");
                        HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, fptEmail);
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var user = JsonConvert.DeserializeObject<User>(jsonString);
                            if (user != null)
                            {
                                HttpContext.Session.Remove("sessionAccount");
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "sessionAccount", user);
                                return Json(true);
                            }
                            else
                            {
                                return Json(2);
                            }
                        }
                        else
                        {
                            return Json(false);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
