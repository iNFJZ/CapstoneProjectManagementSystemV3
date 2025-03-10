using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.AffiliateAccountService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.StudentService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Infrastructure.Services.CommonServices.PasswordHasherService;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Services.PrivateService.RoleService;

namespace CapstoneProjectManagementSystemV3.Controllers.CommonController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IAffiliateAccountService _affiliateAccountService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStudentService _studentService;
        private readonly IRoleService _roleService;
        //ILog log = LogManager.GetLogger(typeof(UserController));
        public string ErrorMessage { get; set; }    //ErrorMessage is used to report the error and push to the client by tempdata
        public string SuccessMessaage { get; set; } //SuccessMessaage is used to report the success and push to the client by tempdata
        public UserController(IAffiliateAccountService affiliateAccountService, IPasswordHasherService passwordHasherService
            , IMailService mailService, IUserService userService, ISessionExtensionService sessionExtensionService
            , IHttpContextAccessor httpContextAccessor, IRoleService roleService
            , IStudentService studentService)

        {
            _affiliateAccountService = affiliateAccountService;
            _passwordHasherService = passwordHasherService;
            _mailService = mailService;
            _userService = userService;
            _sessionExtensionService = sessionExtensionService;
            _httpContextAccessor = httpContextAccessor;
            _studentService = studentService;
            _roleService = roleService;
        }
        [HttpPost("sign-in-ByAffiliateAccount")]
        public async Task<IActionResult> SignInByAffiliateAccount(string personalEmail, string passwordHash)
        {
            try
            {
                var connectionString = GetConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                {
                    return Ok(new ApiErrorResult<dynamic>(new
                    {
                        status = false,
                        mess = "Không có thông tin database",
                        Path = "User/SignInByAffiliateAccount"
                    }));
                }

                //get infor affiliate account by function GetBackupAccountByEmail with parameter is personalEmail
                var affiliateAccount = (await _affiliateAccountService.GetAffiliateAccountByEmail(personalEmail));
                if (affiliateAccount.IsSuccessed == false)
                {
                    return Ok(affiliateAccount);
                }
                // check login with affiliate accoutn and password hash 
                var checkUserLogin = (await _affiliateAccountService.CheckAffiliateAccountAndPasswordHash(personalEmail, passwordHash)).ResultObj;

                //if it return true -> login sucssess and set to session redirect homepage of student 
                if (checkUserLogin == true)
                {
                    var userLogin = await _userService.GetUserByID(affiliateAccount.ResultObj.AffiliateAccountID);
                    //var infor = await _studentService.UpdateSemesterOfStudentByUserId(userLogin.ResultObj.UserID);
                    var role = userLogin.ResultObj.RoleID;
                    _sessionExtensionService.SetObjectAsJson(HttpContext.Session, "User", userLogin);
                    return Ok(new ApiSuccessResult<dynamic>(new
                    {
                        status = true,
                        obj = userLogin,
                        role = role,
                        mess = "Thành công",
                        Path = "StudentHome/Index"
                    }));
                }
                // if it return false -> login error will redirect page SignInByAffiliateAccount with notify error message
                else
                {
                    return Ok(new ApiErrorResult<dynamic>(new
                    {
                        status = false,
                        mess = "Email or Password invalid",
                        Path = "User/SignInByAffiliateAccount"
                    }));
                }
            }
            catch
            {
                return Ok(new ApiErrorResult<dynamic>(new
                {
                    status = false,
                    mess = "Email or Password invalid",
                    Path = "/Views/Common_View/404NotFound.cshtml"
                }));
            }
        }
        [HttpGet("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                HttpContext.Session.Remove("sessionAccount");
                await HttpContext.SignOutAsync();
                return Ok(new ApiSuccessResult<dynamic>(new
                {
                    status = true,
                }));
            }
            catch
            {
                return Ok(new ApiErrorResult<dynamic>(new
                {
                    status = false,
                    mess = "Email or Password invalid",
                    Path = "/Views/Common_View/404NotFound.cshtml"
                }));
            }
        }

    }
}
