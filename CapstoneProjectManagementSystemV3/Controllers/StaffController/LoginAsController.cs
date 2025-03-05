using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.DBContext;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace CapstoneProjectManagementSystemV3.Controllers.StaffController
{
    //[Authorize(Roles ="Staff")]
    [Route("api/[controller]")]
    public class LoginAsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ISessionExtensionService _sessionExtensionService;
        //private readonly ILogger<EditSupervisorController> _logger;
        public LoginAsController(IUserService userService
                                , ISessionExtensionService sessionExtensionService
            //, ILogger<EditSupervisorController> logger
            )
        {
            _userService = userService;
            _sessionExtensionService = sessionExtensionService;
            //_logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAs([FromBody] string fptEmail)
        {
            try
            {
                var connectionString = GetConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                {
                    return Ok(new ApiErrorResult<int>("Không có thông tin database"));
                }
                //using var context = new DBContext(connectionString);

                if (string.IsNullOrWhiteSpace(fptEmail))
                {
                    return Ok(new ApiSuccessResult<dynamic>(new { status = false }));
                }
                var user = await _userService.GetUserByFptEmail(fptEmail.Trim(), 3);
                if (user != null)
                {
                    return Ok(new ApiSuccessResult<dynamic>(new
                    {
                        user.ResultObj
                    }));
                }

                return Ok(new ApiErrorResult<int>("Tài khoản không tồn tại"));

            }
            catch (Exception)
            {
                return Ok(new ApiErrorResult<int>("Email or Password invalid"));

            }
        }
    }
}
