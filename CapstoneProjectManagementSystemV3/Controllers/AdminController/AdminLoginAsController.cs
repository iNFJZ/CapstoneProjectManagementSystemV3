using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.AdminController
{
    [Route("api/admin")]
    [ApiController]
    public class AdminLoginAsController : ControllerBase
    {
        private readonly ILogger<AdminLoginAsController> _logger;
        private readonly IUserService _userService;
        private readonly IDataRetrievalService _dataRetrievalService;

        public AdminLoginAsController(ILogger<AdminLoginAsController> logger, IUserService userService, IDataRetrievalService dataRetrievalService)
        {
            _logger = logger;
            _userService = userService;
            _dataRetrievalService = dataRetrievalService;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("View loginAs screen");
                return Ok(new ApiSuccessResult<dynamic>(new
                {
                    status = true,
                    mess = "View loginAs screen",
                    Path = "/Views/Admin_View/LoginAs/Index.cshtml"
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "View loginAs screen error");
                return Ok(new ApiSuccessResult<dynamic>(new
                {
                    status = false,
                    mess = "Error viewing loginAs screen",
                    Path = "/Views/Common_View/404NotFound.cshtml"
                }));
            }
        }

        [HttpPost("login-as")]
        public IActionResult LoginAs([FromBody] string fptEmail)
        {
            try
            {
                _logger.LogInformation("Login As");
                if (string.IsNullOrWhiteSpace(fptEmail))
                {
                    return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "Email is required" }));
                }

                var user = _userService.GetUserByFptEmail(fptEmail.Trim(), 5);
                if (user != null)
                {
                    _dataRetrievalService.SetData($"user_{fptEmail}", user, TimeSpan.FromHours(2));
                    return Ok(new ApiSuccessResult<dynamic>(new { status = true, mess = "Login successful", user }));
                }
                else
                {
                    return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "User not found" }));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login As error");
                return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "An error occurred during login" }));
            }
        }
    }
}
