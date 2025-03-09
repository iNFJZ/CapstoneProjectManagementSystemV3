using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.AdminViewModel;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.RoleService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminListUserController : ControllerBase
    {
        private readonly ILogger<AdminListUserController> _logger;
        private readonly IUserService _userService;
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly ISupervisorService _supervisorService;
        private readonly IRoleService _roleService;
        public AdminListUserController(ILogger<AdminListUserController> logger,
            IUserService userService,
            IDataRetrievalService dataRetrievalService,
            ISupervisorService supervisorService, IRoleService roleService)
        {
            _logger = logger;
            _userService = userService;
            _dataRetrievalService = dataRetrievalService;
            _supervisorService = supervisorService;
            _roleService = roleService;
        }

        [HttpGet("list-users")]
        public async Task<IActionResult> GetUsers(int page, string? search, int? role)
        {
            try
            {
                _logger.LogInformation("Fetching list of users");
                search = search ?? string.Empty;
                var roles = (await _roleService.GetRoles()).ResultObj;
                int totalPage;
                List<UserWithRowNum> users;
                (totalPage, page, users) = (await _userService.GetListUserForAdminPaging(page, search.Trim(), role ??= 0)).ResultObj;

                return Ok(new { status = true, totalPage, pageIndex = page, search, role, roles, users });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching list of users");
                //return BadRequest(new { status = false, mess = "Error fetching user list" });
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-details")]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            try
            {
                _logger.LogInformation("Fetching user details");
                var roles = await _roleService.GetRoles(); // không hiểu dòng này để làm gì 
                var userDetail = await _userService.GetUserByID(userId);

                return Ok(new ApiResult<dynamic>
                {
                    IsSuccessed = true,
                    //roles,//không hiểu
                    ResultObj = userDetail
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user details");
                return BadRequest(new { status = false, mess = "Error fetching user details" });
            }
        }

        [HttpGet("check-reference")]
        public async Task<IActionResult> CheckUserReference(string userId)
        {
            try
            {
                _logger.LogInformation("Checking reference for user deletion");
                var user = (await _userService.GetUserByID(userId)).ResultObj;
                bool checkReference = (await _userService.CheckReferenceDUserData(user)).IsSuccessed;

                if (user.Role.Role_ID == 3)
                {
                    return Ok(new { status = checkReference ? 0 : 2 });
                }
                else if (user.Role.Role_ID == 4)
                {
                    return Ok(new { status = checkReference ? 1 : 2 });
                }
                return Ok(new ApiResult<dynamic> { IsSuccessed = false, Message = "Invalid user role" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user reference");
                return BadRequest(new { status = false, mess = "Error checking reference data" });
            }
        }

    }
}