
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
using Infrastructure.Entities.Common.ApiResult;

namespace CapstoneProjectManagementSystem.Controllers.Common_Controller
{
    public class UserController : Controller
    {
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly IConfiguration _configuration;
        [TempData]
        public string ErrorMessage { get; set; }    //ErrorMessage is used to report the error and push to the client by tempdata
        public string SuccessMessaage { get; set; } //SuccessMessaage is used to report the success and push to the client by tempdata
        public UserController(
                                ISessionExtensionService sessionExtensionService
            , IConfiguration configuration
            )
        {
            _sessionExtensionService = sessionExtensionService;
            _configuration = configuration;
        }

        public IActionResult SignIn() // view page signin 
        {
            try
            {
                var user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");

                if (user == null)
                {
                    return View("/views/common_view/404NotFound.cshtml");
                }
                else
                {
                    if (user.Role.RoleId == 1)
                    {
                        return RedirectToAction("index", "studenthome");
                    }
                    else if (user.Role.RoleId == 3)
                    {
                        return RedirectToAction("index", "semestermanage");
                    }
                    else if (user.Role.RoleId == 5)
                    {
                        return RedirectToAction("index", "listuser");
                    }
                    else if (user.Role.RoleId == 4)
                    {
                        return RedirectToAction("index", "createideadevhead");
                    }
                    else
                    {
                        return RedirectToAction("index", "createideasupervisor");
                    }
                }
                return View("/Views/Common_View/SignIn.cshtml");
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
                var user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");
                if (user == null)
                {
                    return View("/Views/Common_View/SignInByAffiliateAccount.cshtml");
                }
                else
                {
                    if (user.Role.RoleId == 1)
                    {
                        return RedirectToAction("index", "studenthome");
                    }
                    else if (user.Role.RoleId == 3)
                    {
                        return RedirectToAction("index", "semestermanage");
                    }
                    else if (user.Role.RoleId == 5)
                    {
                        return RedirectToAction("index", "listuser");
                    }
                    else if (user.Role.RoleId == 4)
                    {
                        return RedirectToAction("index", "createideadevhead");
                    }
                    else
                    {
                        return RedirectToAction("index", "createideasupervisor");
                    }
                }
                return View("/Views/Common_View/SignInByAffiliateAccount.cshtml");
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
                    client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                    client.DefaultRequestHeaders.Add("X-Connection-String", campus);
                    string apiUrl = $"/api/User/sign-in-ByAffiliateAccount?personalEmail={personalEmail}&passwordHash={passwordHash}";
                    HttpResponseMessage response = await client.PostAsync(apiUrl, null);
                    if (response.IsSuccessStatusCode)
                    {

                        var jsonString = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<ApiResult<User>>(jsonString);
                        user.ResultObj.Role.Users = null;
                        var userSS = new User()
                        {
                            Avatar = user.ResultObj.Avatar,
                            UserId = user.ResultObj.UserId,
                            UserName = user.ResultObj.UserName,
                        };
                        if (user != null)
                        {
                            if(user.ResultObj.Role.RoleId == 1)
                            {
                                //HttpContext.Session.Remove("sessionAccount");
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "campus", campus);
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "sessionAccount", userSS);
                                //return RedirectToAction("Index", "StudentHome");
                                return RedirectToAction("Index", "StudentHome");
                            }
                            else if (user.ResultObj.Role.RoleId == 2)
                            {
                                //HttpContext.Session.Remove("sessionAccount");
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "campus", campus);
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "sessionAccount", userSS);
                                //return RedirectToAction("Index", "StudentHome");
                                return RedirectToAction("Index", "SupervisorHome");
                            }
                            else if(user.ResultObj.Role.RoleId == 3)
                            {
                                //HttpContext.Session.Remove("sessionAccount");
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "campus", campus);
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "sessionAccount", userSS);
                                //return RedirectToAction("Index", "StudentHome");
                                return RedirectToAction("Index", "StaffHome");
                            }
                            else if(user.ResultObj.Role.RoleId == 4)
                            {
                                //HttpContext.Session.Remove("sessionAccount");
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "campus", campus);
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "sessionAccount", userSS);
                                //return RedirectToAction("Index", "StudentHome");
                                return RedirectToAction("Index", "DebHeadHome");
                            }
                            else if(user.ResultObj.Role.RoleId == 5)
                            {
                                //HttpContext.Session.Remove("sessionAccount");
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "campus", campus);
                                _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "sessionAccount", userSS);
                                //return RedirectToAction("Index", "StudentHome");
                                return RedirectToAction("Index", "AdminHome");
                            }
                            return RedirectToAction("SignInByAffiliateAccount", "User", new { message = ErrorMessage });
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
