using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.FinalGroupService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.PrivateService.StaffService;
using Infrastructure.Services.PrivateService.StudentGroupIdeaService;
using Infrastructure.Services.PrivateService.StudentService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text.Json;
using Infrastructure.Entities.Common;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Entities.Dto.ViewModel.AdminViewModel;

namespace CapstoneProjectManagementSystemV3.Controllers.CommonController
{
    [Route("api/[controller]")]
    [ApiController]

    public class ExternalSignInController : BaseApiController
    {
        private readonly IStudentService _studentService;
        private readonly IStudentGroupIdeaService _studentGroupIdeaService;
        private readonly IStaffService _staffService;
        private readonly ISemesterService _semesterService;
        private readonly IUserService _userService;
        private readonly IFinalGroupService _finalGroupService;
        private readonly ILogger<ExternalSignInController> _logger;
        private readonly IDataRetrievalService _dataRetrievalService;
        public ExternalSignInController(IStudentService studentService,
            IStudentGroupIdeaService studentGroupIdeaService,
            IStaffService staffService,
            ISemesterService semesterService,
            IUserService userService,
            IFinalGroupService finalGroupService,
            ILogger<ExternalSignInController> logger,
            IDataRetrievalService dataRetrievalService)
        {
            _studentService = studentService;
            _studentGroupIdeaService = studentGroupIdeaService;
            _staffService = staffService;
            _semesterService = semesterService;
            _userService = userService;
            _finalGroupService = finalGroupService;
            _logger = logger;
            _dataRetrievalService = dataRetrievalService;
        }
        [HttpPost("signin-google")]
        public async Task<IActionResult> SignInGoogle(string returnUrl, string token, string campus)
        {
            try
            {
                string checkPoint = $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}";
                using var client = new HttpClient();
                var response = await client.GetStringAsync(checkPoint);
                var googleAuth = JsonSerializer.Deserialize<GoogleAuthenClass>(response);

                if (googleAuth?.email == null)
                {
                    await HttpContext.SignOutAsync();
                    return BadRequest(new ApiSuccessResult<string>("Error loading external login information"));
                }

                var loginInfo = new User
                {
                    UserId = googleAuth.email,
                    FptEmail = googleAuth.email,
                    FullName = googleAuth.given_name,
                    UserName = googleAuth.email[..^11],
                    Avatar = googleAuth.picture,
                };

                var curSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (curSemester != null && !(await _studentService.IsFirstSemesterDoCapstoneProject(loginInfo.UserId, curSemester.SemesterID)).IsSuccessed)
                {
                    loginInfo.UserId += $"-{curSemester.SemesterID}";
                }

                var existingUser = (await _userService.GetUserByID(loginInfo.UserId)).ResultObj;
                if (existingUser != null)
                {
                    if (existingUser.Role.Role_ID == 3) //staff
                    {
                        var role = new Role()
                        {
                            RoleId = 3
                        };
                        loginInfo.Role = role; // role staff
                        await _userService.UpdateAvatar(loginInfo.Avatar, loginInfo.UserId);
                        //_dataRetrievalService.SetData("sessionAccount", loginInfo);

                        return Ok(new ApiSuccessResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "SemesterManage/Index", Mess = "" }));
                    }
                    else if (existingUser.Role.Role_ID == 2)//Update login by role staff 2/6/2023
                    {
                        //Current semester not start return Login with message
                        if (curSemester == null)
                        {
                            HttpContext.Session.Remove("sessionAccount");
                            await HttpContext.SignOutAsync();
                            return Ok(new ApiErrorResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "User/SignIn", Mess = "Semester not start!" }));
                            //return RedirectToAction("SignIn", "User", new { message = ErrorMessage });
                        }
                        var role = new Role()
                        {
                            RoleId = 2
                        };
                        loginInfo.Role = role; // role mentor
                        _userService.UpdateAvatar(loginInfo.Avatar, loginInfo.UserId);
                        return Ok(new ApiSuccessResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "HomeSupervisor/Index" }));
                    }
                    else if (existingUser.Role.Role_ID == 4)
                    {
                        //Current semester not start return Login with message
                        if (curSemester == null)
                        {
                            HttpContext.Session.Remove("sessionAccount");
                            await HttpContext.SignOutAsync();
                            //ErrorMessage = "Semester not start!";
                            return Ok(new ApiErrorResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "User/SignIn", Mess = "Semester not start!" }));
                        }
                        var role = new Role()
                        {
                            RoleId = 4
                        };
                        loginInfo.Role = role; // role devhead
                        _userService.UpdateAvatar(loginInfo.Avatar, loginInfo.UserId);
                        return Ok(new ApiSuccessResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "HomeDevHead/Index" }));
                    }
                    else if (existingUser.Role.Role_ID == 5)
                    {
                        var role = new Role()
                        {
                            RoleId = 5
                        };
                        loginInfo.Role = role; // role admin
                        _userService.UpdateAvatar(loginInfo.Avatar, loginInfo.UserId);
                        return Ok(new ApiSuccessResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "ListUser/Index" }));
                    }
                }

                if (Regex.IsMatch(loginInfo.FptEmail, "[A-Za-z][0-9]{5,}@fpt.edu.vn"))
                {
                    if (curSemester == null)
                    {
                        return Ok(new ApiErrorResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "SignIn/User", Mess = "Semester not started!" }));
                    }
                    loginInfo.Role = new Role { RoleId = 1 };
                    return Ok(new ApiSuccessResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "StudentHome/Index", Mess = "Student logged in successfully" }));

                }
                return Ok(new ApiErrorResult<UserViewModel>(new UserViewModel { User = loginInfo, Path = "SignIn/User", Mess = "Your account does not have access to the system" }));

            }
            catch (Exception ex)
            {
                await HttpContext.SignOutAsync();
                return BadRequest(new ApiSuccessResult<string>(ex.Message));
            }
        }
    }
}
}