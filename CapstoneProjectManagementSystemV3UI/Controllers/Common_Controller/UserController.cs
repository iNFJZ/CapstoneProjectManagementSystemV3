
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Infrastructure.Entities;
using Newtonsoft.Json;
using Infrastructure.Services.CommonServices.SessionExtensionService;

namespace CapstoneProjectManagementSystem.Controllers.Common_Controller
{
    public class UserController : Controller
    {
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;

        [TempData]
        public string ErrorMessage { get; set; }    //ErrorMessage is used to report the error and push to the client by tempdata
        public string SuccessMessaage { get; set; } //SuccessMessaage is used to report the success and push to the client by tempdata
        public UserController( ISessionExtensionService sessionExtensionService , IConfiguration configuration, ILogger<UserController> logger)
        {
            _sessionExtensionService = sessionExtensionService;
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult SignIn() // view page signin 
        {
            try
            {
                //var user = _sessionExtensionService.GetObjectFromJson<User>(Httpontext.Session, "sessionAccount");
                return View("/Views/Common_View/SignIn.cshtml");
                //if (user == null)
                //{
                //    return View("/Views/Common_View/SignIn.cshtml");
                //}
                //else
                //{
                //    if (user.Role.Role_ID == 1)
                //    {
                //        return RedirectToAction("Index", "StudentHome");
                //    }
                //    else if (user.Role.Role_ID == 3)
                //    {
                //        return RedirectToAction("Index", "SemesterManage");
                //    }
                //    else if (user.Role.Role_ID == 5)
                //    {
                //        return RedirectToAction("Index", "ListUser");
                //    }
                //    else if (user.Role.Role_ID == 4)
                //    {
                //        return RedirectToAction("Index", "CreateIdeaDevHead");
                //    }
                //    else
                //    {
                //        return RedirectToAction("Index", "CreateIdeaSupervisor");
                //    }
                //}
            }
            catch
            {
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }
        [HttpGet]
        public IActionResult SignInByAffiliateAccount() // view sign in with account registered in profile
        {
            try
            {
                return View("/Views/Common_View/SignInByAffiliateAccount.cshtml");

                //var user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");
                //if (user == null)
                //{
                //    return View("/Views/Common_View/SignInByAffiliateAccount.cshtml");
                //}
                //else
                //{
                //    if (user.Role.Role_ID == 1)
                //    {
                //        return RedirectToAction("Index", "StudentHome");
                //    }
                //    else if (user.Role.Role_ID == 3)
                //    {
                //        return RedirectToAction("Index", "SemesterManage");
                //    }
                //    else if (user.Role.Role_ID == 2)
                //    {
                //        return RedirectToAction("Index", "ManageMentor");
                //    }
                //    else if (user.Role.Role_ID == 5)
                //    {
                //        return RedirectToAction("Index", "ListUser");
                //    }
                //    else if (user.Role.Role_ID == 4)
                //    {
                //        return RedirectToAction("Index", "SupervisorChangeTopicRequest");
                //    }
                //    else
                //    {
                //        return RedirectToAction("Index", "CreateIdeaSupervisor");
                //    }
                //}
            }
            catch
            {
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }


        [HttpPost]
        public async Task<IActionResult> SignInByAffiliateAccount(string personalEmail, string passwordHash, string campus)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (campus != "" && campus != null)
                    {
                        HttpContext.Session.SetString("campus", campus);
                    }
                    else
                    {
                        ErrorMessage = "Please select campus!";
                        _logger.LogError(ErrorMessage);
                        return RedirectToAction("SignInByAffiliateAccount", "User", new { message = ErrorMessage });
                    }
                    client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                    client.DefaultRequestHeaders.Add("X-Connection-String", campus);
                    string apiUrl = $"/api/User/sign-in-ByAffiliateAccount?personalEmail={personalEmail}&passwordHash={passwordHash}";
                    HttpResponseMessage response = await client.PostAsync(apiUrl, null);
                    if (response.IsSuccessStatusCode)
                    {

                        var jsonString = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<User>(jsonString);
                        if (user != null)
                        {
                            //HttpContext.Session.Remove("sessionAccount");
                            _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "campus", campus);
                            _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "sessionAccount", user);
                            //return RedirectToAction("Index", "StudentHome");
                            return View("/Views/Admin_View/ListUser/Index.cshtml");
                        }
                        else
                        {
                            ErrorMessage = "Email or Password invalid";
                            TempData["oldPersonalEmail"] = personalEmail;
                            return RedirectToAction("SignInByAffiliateAccount", "User", new { message = ErrorMessage });
                        }
                    }
                    else
                    {
                        ErrorMessage = "Email or Password invalid";
                        TempData["oldPersonalEmail"] = personalEmail;
                        return RedirectToAction("SignInByAffiliateAccount", "User", new { message = ErrorMessage });
                    }
                }
            }
            catch
            {
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }

        public IActionResult ForgotPassword()
        {
            try
            {
                var user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");
                if (user == null)
                {
                    return View("/Views/Common_View/ForgotPassword.cshtml");
                }
                else
                {
                    if (user.Role.RoleId == 1)
                    {
                        return RedirectToAction("Index", "StudentHome");
                    }
                    else if (user.Role.RoleId == 3)
                    {
                        return RedirectToAction("Index", "SemesterManage");
                    }
                    else if (user.Role.RoleId == 2)
                    {
                        return RedirectToAction("Index", "ManageMentor");
                    }
                    else if (user.Role.RoleId == 5)
                    {
                        return RedirectToAction("Index", "ListUser");
                    }
                    else if (user.Role.RoleId == 4)
                    {
                        return RedirectToAction("Index", "SupervisorChangeTopicRequest");
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }

        public async Task<IActionResult> SignOut()
        {
            try
            {
                HttpContext.Session.Remove("sessionAccount");
                await HttpContext.SignOutAsync();
                return Redirect("~/");
            }
            catch
            {
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }
    }
}
