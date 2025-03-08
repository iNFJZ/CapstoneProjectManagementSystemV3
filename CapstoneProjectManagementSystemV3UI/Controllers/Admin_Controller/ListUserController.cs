
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using DocumentFormat.OpenXml.InkML;
using Infrastructure.Repositories.PasswordHash;
using Newtonsoft.Json;
using CapstoneProjectManagementSystemV3UI.Models;
using Infrastructure.Entities;

namespace CapstoneProjectManagementSystem.Controllers.Admin_Controller
{
    //[Authorize(Roles = "Admin")]
    public class ListUserController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<ListUserController> logger;
        private readonly ISessionExtensionService _sessionExtensionService;

        public ListUserController(
            ILogger<ListUserController> logger,
            IConfiguration configuration,
            ISessionExtensionService sessionExtensionService
            )
        {
            this.logger = logger;
            this._sessionExtensionService = sessionExtensionService;
            this._configuration = configuration;
        }

        public async Task<IActionResult> Index(int page, string search, int role)
        {
            try
            {
                logger.LogInformation("View List User Screen");
                if (search == null) search = "";
                var campus = _sessionExtensionService.GetObjectFromJson<string>(HttpContext.Session, "campus");
                if(campus == null)
                {
                    _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "campus", "HL");
                    return View("/Views/Admin_View/LoginAs/Index.cshtml");
                }
                List<CapstoneProjectManagementSystemV3UI.Models.Role> roles = new List<CapstoneProjectManagementSystemV3UI.Models.Role>();
                int totalPage = 0;
                List<User> users = new List<User>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["BaseAddress"]);
                    client.DefaultRequestHeaders.Add("X-Connection-String", campus);
                    string apiUrl = $"/api/AdminListUser/list-users?page={page}&search={role}&role={role}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var body = JsonConvert.DeserializeObject<ListUserViewModel>(jsonString);
                        roles = body.resultObj.roles ?? new List<CapstoneProjectManagementSystemV3UI.Models.Role>();
                        users = body.resultObj.users ?? new List<User>();
                    }
                }
                ViewBag.roles = roles;
                ViewBag.users = users;
                ViewBag.totalPage = totalPage;
                ViewBag.pageIndex = page;
                ViewBag.search = search;
                ViewBag.role = role;

                return View("Views/Admin_View/ListUser/Index.cshtml");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "View List User Screen error");
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }

        public IActionResult Details(string userId) 
        {
            try
            {
                //logger.LogInformation("View List User Screen");
               
                //List<Role> roles;
                //roles = roleService.GetRoles();
                //ViewBag.roles = roles;
                //int totalPage = 0;

                //var userDetail = userService.GetUserByID(userId);
                //ViewBag.user = userDetail;

                return View("Views/Admin_View/ListUser/Details.cshtml");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "View List User Screen error");
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }
        public JsonResult CheckReference(string userId)
        {
            try
            {
                //logger.LogInformation("Check Reference to delete user");
                //User user = userService.GetUserByID(userId);
                //bool checkReference = userService.CheckReferenceDUserData(user);

                //if (user.Role.Role_ID == 3)
                //{
                //    if (checkReference)
                //        return Json(0); // warning: staff have reference data
                //    else
                //        return Json(2); // no reference data
                //}
                //else if (user.Role.Role_ID == 4)
                //{
                //    if (checkReference)
                //        return Json(1); // message: can not detele because this account have reference data
                //    else
                //        return Json(2); // no reference data
                //}
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Check Reference error");
                return null;
            }
        }

        public JsonResult DeleteUser(string userId)
        {
            try
            {
                //logger.LogInformation("Delete User");
                //User user = userService.GetUserByID(userId);
                //int delete = userService.DeleteUser(user);

                //if (delete == 2)
                //{
                //    return Json(1); // delete success
                //}

                return Json(0);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Delete User error");
                return Json(0);
            }
        }

        //[HttpPost]
        //public JsonResult UpdateRoleUser([FromBody] User userRole)
        //{
        //    try
        //    {

        //        return Json(userRole);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Update User error");
        //        return Json(0);
        //    }
        //}


    }
}
