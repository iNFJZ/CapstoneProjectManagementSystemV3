
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CapstoneProjectManagementSystem.Controllers.Common_Controller
{


    public class ExternalSignInController : Controller
    {
        private readonly ILogger<ExternalSignInController> _logger;
        public ExternalSignInController(ILogger<ExternalSignInController> logger)
        {
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignInGoogle(string returnUrl, string token, string campus)
        {
            try
            {
                //if (loginEmail == null)
                //{
                //    await HttpContext.SignOutAsync();
                //    ErrorMessage = "Error loading external login information";
                //    return RedirectToAction("SignIn", "User", new { message = ErrorMessage });
                //}
                return RedirectToAction("SignIn", "User", new { message = ErrorMessage });

            }
            catch (Exception ex)
            {
                HttpContext.Session.Remove("sessionAccount");
                await HttpContext.SignOutAsync();
                ErrorMessage = ex.Message;
                return RedirectToAction("SignIn", "User", new
                {
                    message = ErrorMessage
                });
            }
        }
    }
}
