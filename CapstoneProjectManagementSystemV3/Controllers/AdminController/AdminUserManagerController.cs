using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.RoleService;
using Infrastructure.Services.PrivateService.StaffService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.CommonServices.DataRetrievalService;

namespace CapstoneProjectManagementSystemV3.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserManagerController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly INotificationService _notificationService;
        private readonly IStaffService _staffService;
        private readonly ISupervisorService _supervisorService;
        private readonly ILogger<AdminUserManagerController> _logger;
        private readonly IDataRetrievalService _dataRetrievalService;
        public AdminUserManagerController(IUserService userService, IRoleService roleService,
                                INotificationService notificationService,
                                IStaffService staffService,
                                ISupervisorService supervisorService, 
                                ILogger<AdminUserManagerController> logger,
                                IDataRetrievalService dataRetrievalService)
        {
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
            _notificationService = notificationService;
            _staffService = staffService;
            _supervisorService = supervisorService;
            _dataRetrievalService = dataRetrievalService;
        }
        [HttpGet("admin_index")]
        public async Task<IActionResult> GetUserList(string username)
        {
            try
            {
                _logger.LogInformation("Fetching user list");
                var user = _dataRetrievalService.GetData<User>("sessionAccount");

                var roles =( await _roleService.GetRoles()).ResultObj.ToList();
                var listRoleId = roles.Select(x => x.RoleId).Where(x => x != 1).ToList();
                var listUser = await _userService.GetListUserByRoleList(listRoleId);

                return Ok(new ApiSuccessResult<dynamic>(new
                {
                    status = true,
                    sessionRole = user.Role.RoleId,
                    roles,
                    users = listUser
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user list");
                return BadRequest(new ApiSuccessResult<dynamic>(new { status = false, mess = "Error fetching user list" }));
            }
        }
        [HttpGet]
        [Route("admin_users")]
        public async Task<IActionResult> GetUsers(string username)
        {
            try
            {
                _logger.LogInformation("User API - Get Users");

                // Lấy user từ session
                //user user = sessionextensionservice.getobjectfromjson<user>(httpcontext.session, "sessionaccount");
                //if (user == null)
                //{
                //    return ok(new apisuccessresult<bool>(false));
                //}
                var user = _dataRetrievalService.GetData<User>("X-User-Info");

                if (user == null)
                {
                    return Ok(new ApiSuccessResult<bool>(false));
                }
                // Gọi API lấy danh sách Role
                var rolesResult = await _roleService.GetRoles();
                if (!rolesResult.IsSuccessed || rolesResult.ResultObj == null)
                {
                    return Ok(new ApiSuccessResult<bool>(false));
                }
                List<Role> roles = rolesResult.ResultObj;

                // Lọc danh sách Role ID không phải 1
                List<int> listRoleId = roles.Select(x => x.RoleId).Where(x => x != 1).ToList();

                // Gọi API lấy danh sách User theo Role ID
                var usersResult = await _userService.GetListUserByRoleList(listRoleId);
                if (!usersResult.IsSuccessed || usersResult.ResultObj == null)
                {
                    return Ok(new ApiSuccessResult<bool>(false));
                }

                return Ok(new ApiSuccessResult<bool>(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API Get Users Error");
                return StatusCode(500, new ApiSuccessResult<bool>(false));
            }
        }
        [HttpGet("check-reference/{userId}")]
        public async Task<IActionResult> CheckReference(string userId)
        {
            try
            {
                _logger.LogInformation("Checking reference for user deletion");
                var user = (await _userService.GetUserByID(userId)).ResultObj;
                bool checkReference = (await _userService.CheckReferenceDUserData(user)).IsSuccessed;

                int result = user.Role.RoleId switch
                {
                    3 => checkReference ? 0 : 2,
                    4 => checkReference ? 1 : 2,
                    _ => -1
                };

                return Ok(new ApiSuccessResult<int>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking reference");
                return BadRequest(new ApiSuccessResult<int>(-1));
            }
        }

        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                _logger.LogInformation("Deleting user");
                var user = (await _userService.GetUserByID(userId)).ResultObj;
                bool delete = (await _userService.DeleteUser(user)).IsSuccessed;

                return Ok(new ApiSuccessResult<bool>(delete));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return BadRequest(new ApiSuccessResult<int>(0));
            }
        }

        [HttpPost("update-role-user")]
        public async Task<IActionResult> UpdateRoleUser([FromBody] User userRole)
        {
            try
            {
                return Ok(new ApiSuccessResult<User>(userRole));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user role");
                return BadRequest(new ApiSuccessResult<int>(0));
            }
        }
    }
}
