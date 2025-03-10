using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminCreateUserController : ControllerBase
    {
        private readonly ILogger<AdminCreateUserController> _logger;
        private readonly IUserService _userService;
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly ISupervisorService _supervisorService;

        public AdminCreateUserController(ILogger<AdminCreateUserController> logger, IUserService userService, IDataRetrievalService dataRetrievalService, ISupervisorService supervisorService)
        {
            _logger = logger;
            _userService = userService;
            _dataRetrievalService = dataRetrievalService;
            _supervisorService = supervisorService;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("View Create User Screen");
                return Ok(new ApiSuccessResult<dynamic>(new { status = true, mess = "View Create User Screen" }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "View Create User Screen error");
                return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "Error viewing Create User screen" }));
            }
        }

        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] UserCreateRequest user)
        {
            try
            {
                _logger.LogInformation("Create Staff");
                bool checkDuplicateUser = (await _userService.checkDuplicateUser(user.FptEmail.Trim())).ResultObj;
                if (checkDuplicateUser == true)
                    return Ok(new ApiErrorResult<dynamic>(new { status = false, mess = "User already exists" }));

                if ((await _userService.CreateStaffForAdmin(user)).ResultObj == true)
                {
                    return Ok(new ApiSuccessResult<dynamic>(new { status = true, mess = "Staff created successfully" }));
                }
                return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "Failed to create staff" }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Staff error");
                return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "An error occurred while creating staff" }));
            }
        }

        [HttpPost("create-supervisor-leader")]
        public async Task<IActionResult> CreateSupervisorLeader([FromBody] SupervisorDto supervisor)
        {
            try
            {
                _logger.LogInformation("Create Department Leader");
                bool checkDuplicateUser = (await _userService.checkDuplicateUser(supervisor.SupervisorNavigation.FptEmail.Trim())).IsSuccessed;
                bool checkDuplicateFEMail = !string.IsNullOrWhiteSpace(supervisor.FeEduEmail) &&(await _supervisorService.checkDuplicateFEEduEmail(supervisor.FeEduEmail.Trim())).IsSuccessed;

                if (checkDuplicateFEMail)
                    return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "FE Email already exists" }));

                if (checkDuplicateUser)
                    return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "User already exists" }));

                if (!checkDuplicateUser && !checkDuplicateFEMail)
                {
                    if ((await _userService.CreateSupervisorLeaderForAdmin(supervisor)).IsSuccessed)
                    {
                        return Ok(new ApiSuccessResult<dynamic>(new { status = true, mess = "Department Leader created successfully" }));
                    }
                }
                return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "Failed to create Department Leader" }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Department Leader error");
                return Ok(new ApiSuccessResult<dynamic>(new { status = false, mess = "An error occurred while creating Department Leader" }));
            }
        }
    }
}
